import React, { Component } from "react";
import { translate } from 'react-i18next';
import { withRouter, NavLink } from 'react-router-dom';
import { withStyles } from 'material-ui/styles';
import moment from 'moment';
import Table, { TableBody, TableCell, TableHead, TableRow, TableFooter, TablePagination, TableSortLabel } from 'material-ui/Table';
import { Popover, IconButton, Menu, MenuItem, Checkbox, TextField, Select, CircularProgress, Grid, Button } from 'material-ui';
import Input, { InputLabel } from 'material-ui/Input';
import Dialog, { DialogTitle, DialogContent, DialogActions } from 'material-ui/Dialog';
import { FormControl, FormHelperText } from 'material-ui/Form';
import MoreVert from '@material-ui/icons/MoreVert';
import Delete from '@material-ui/icons/Delete';
import Search from '@material-ui/icons/Search';
import Visibility from '@material-ui/icons/Visibility';
import AppDispatcher from '../../appDispatcher';
import Constants from '../../constants';
import { ScopeService } from '../../services';
import { SessionStore } from '../../stores';

const styles = theme => ({
  margin: {
    margin: theme.spacing.unit,
  }
});

class ViewScopes extends Component {
    constructor(props) {
        super(props);
        this.handleClick = this.handleClick.bind(this);
        this.handleClose = this.handleClose.bind(this);
        this.handleChangeProperty = this.handleChangeProperty.bind(this);
        this.handleChangeScopeProperty = this.handleChangeScopeProperty.bind(this);
        this.handleChangeFilter = this.handleChangeFilter.bind(this);
        this.refreshData = this.refreshData.bind(this);
        this.handleChangePage = this.handleChangePage.bind(this);
        this.handleChangeRowsPage = this.handleChangeRowsPage.bind(this);
        this.handleRowClick = this.handleRowClick.bind(this);
        this.handleRemoveScopes = this.handleRemoveScopes.bind(this);
        this.handleAllSelections = this.handleAllSelections.bind(this);
        this.handleSort = this.handleSort.bind(this);
        this.handleCloseModal = this.handleCloseModal.bind(this);
        this.handleOpenModal = this.handleOpenModal.bind(this);
        this.handleAddScope = this.handleAddScope.bind(this);

        this.state = {
        	type: '',
            data: [],
            page: 0,
            pageSize: 5,
            count: 0,
            selectedType: 'all',
            selectedName: '',
            isModalOpened: false,
            isLoading: false,
            isSettingsOpened: false,
            isRemoveDisplayed: false,
            isAddScopeLoading: false,
            anchorEl: null,
            order: 'desc',
            newScope: {
                name: '',
                description: ''
            }
        };
    }

    /**
    * Open the menu.
    */
    handleClick(e) {
        this.setState({
            anchorEl: e.currentTarget
        });
    }

    /**
    * Close the menu.
    */
    handleClose() {
        this.setState({
            anchorEl: null
        });
    }

    /**
    * Handle the changes.
    */
    handleChangeProperty(e) {
        var self = this;
        self.setState({
            [e.target.name]: e.target.value
        });
    }

    /**
    * Change the property of the new scope.
    */
    handleChangeScopeProperty(e) {
        var self = this;
        var newScope = self.state.newScope;
        newScope[e.target.name] = e.target.value;
        self.setState({
            newScope: newScope
        });
    }

    /**
    * Handle the filter.
    */
    handleChangeFilter(e) {
        var self = this;
        self.setState({
            [e.target.name]: e.target.value
        }, () => {
            self.refreshData();
        });
    }

    /**
    * Display the data.
    */
    refreshData() {        
        var self = this;
        var startIndex = self.state.page * self.state.pageSize;
        const { t } = self.props;
        self.setState({
            isLoading: true
        });
        var request = { start_index: startIndex, count: self.state.pageSize, order: { target: 'update_datetime', type: (self.state.order === 'desc' ? 1 : 0) } };
        if (self.state.selectedName && self.state.selectedName !== '') {
            request['names'] = [ self.state.selectedName ];
        }

        if (self.state.selectedType !== 'all') {
            request['types'] = [ self.state.selectedType ];
        }

        ScopeService.search(request, self.state.type).then(function (result) {
            var data = [];
            if (result.content) {
                result.content.forEach(function (client) {
                    var type = client.type;
                    type = type === 1 ? t('openidScope') : t('apiScope');
                    data.push({
                        name: client['name'],
                        type: type,
                        isSelected: false,
                        update_datetime: client['update_datetime']
                    });
                });
            }
            
            self.setState({
                isLoading: false,
                data: data,
                count: result.count
            });
        }).catch(function (e) {
            self.setState({
                isLoading: false,
                data: [],
                count: 0
            });
        });
    }

    /**
    * Execute when the page has changed.
    */
    handleChangePage(evt, page) {
        var self = this;
        this.setState({
            page: page
        }, () => {
            self.refreshData();
        });
    }

    /**
    * Execute when the number of records has changed.
    */
    handleChangeRowsPage(evt) {
        var self = this;
        self.setState({
            pageSize: evt.target.value
        }, () => {
            self.refreshData();
        });
    }

    /**
    * Handle click on the row.
    */
    handleRowClick(e, record) {
        record.isSelected = e.target.checked;
        var data = this.state.data;
        var nbSelectedRecords = data.filter(function(r) { return r.isSelected; }).length;
        this.setState({
            data: data,
            isRemoveDisplayed: nbSelectedRecords > 0
        });
    }

    /**
    * Remove the selected scopes.
    */
    handleRemoveScopes() {
        var self = this;
        var scopeNames = self.state.data.filter(function(s) { return s.isSelected; }).map(function(s) { return s.name; });
        if (scopeNames.length === 0) {
            return;
        }

        const { t } = self.props;
        self.setState({
            isLoading: true
        });
        var operations = [];
        scopeNames.forEach(function(scopeName) {
            operations.push(ScopeService.delete(scopeName, self.state.type));
        });

        Promise.all(operations).then(function() {
            AppDispatcher.dispatch({
                actionName: Constants.events.DISPLAY_MESSAGE,
                data: t('scopesAreRemoved')
            });
            self.refreshData();
        }).catch(function(e) {
            self.setState({
                isLoading: false
            });
            AppDispatcher.dispatch({
                actionName: Constants.events.DISPLAY_MESSAGE,
                data: t('scopesCannotBeRemoved')
            });
        });
    }

    /**
    * Select / Unselect all scopes.
    */
    handleAllSelections(e) {
        var checked = e.target.checked;
        var data = this.state.data;
        data.forEach(function(r) { r.isSelected = checked ;});
        this.setState({
            data: data,
            isRemoveDisplayed: checked
        });
    }

    /**
    * Sort the scopes.
    */
    handleSort(colName) {
        var self = this;
        var order = this.state.order === 'desc' ? 'asc' : 'desc';
        this.setState({
            order: order
        }, () => {
            self.refreshData();
        });
    }

    /**
    * Close the modal.
    */
    handleCloseModal() {
        this.setState({
            isModalOpened: false
        });
    }

    /**
    * Open the modal.
    */
    handleOpenModal() {
        this.setState({
            isModalOpened: true
        });
    }

    /**
    * Add the scope.
    */
    handleAddScope() {
        var self = this;
        const {t} = self.props;
        self.setState({
            isAddScopeLoading: true
        });
        ScopeService.add(self.state.newScope, self.state.type).then(function() {
            self.setState({
                isAddScopeLoading: false,
                newScope: {
                    name: '',
                    description: ''
                },
                isModalOpened: false
            });
            self.refreshData();
            AppDispatcher.dispatch({
                actionName: Constants.events.DISPLAY_MESSAGE,
                data: t('scopeAdded')
            });
        }).catch(function(e) {
            self.setState({
                isAddScopeLoading: false
            });
            AppDispatcher.dispatch({
                actionName: Constants.events.DISPLAY_MESSAGE,
                data: t('scopeCannotBeAdded')
            });
        });
    }

    render() {
        var self = this;
        const { t, classes } = this.props;
        var rows = [];
        if (self.state.data) {
            self.state.data.forEach(function(record) {
                rows.push((
                    <TableRow hover role="checkbox" key={record.name}>
                        <TableCell><Checkbox color="primary" checked={record.isSelected} onChange={(e) => self.handleRowClick(e, record)} /></TableCell>
                        <TableCell>{record.name}</TableCell>
                        <TableCell>{record.type}</TableCell>
                        <TableCell>{moment(record.update_datetime).format('LLLL')}</TableCell>
                        <TableCell>
                            <IconButton onClick={ () => self.props.history.push('/' + self.state.type + '/scopes/' + record.name) }><Visibility /></IconButton>
                        </TableCell>
                    </TableRow>
                ));
            });
        }
        
        return (<div className="block">
            <Dialog open={self.state.isModalOpened} onClose={this.handleCloseModal}>                
                <DialogTitle>{t('addScope')}</DialogTitle>
                { self.state.isAddScopeLoading ? (<DialogContent><CircularProgress /></DialogContent>) : (
                    <div>
                        <DialogContent>
                            {/* Name */}
                            <FormControl fullWidth={true} className={classes.margin}>
                                <InputLabel>{t('scopeName')}</InputLabel>
                                <Input value={self.state.newScope.name} name="name" onChange={self.handleChangeScopeProperty}  />
                                <FormHelperText>{t('scopeNameDescription')}</FormHelperText>
                            </FormControl>
                            {/* Description */}
                            <FormControl fullWidth={true} className={classes.margin}>
                                <InputLabel>{t('scopeDescription')}</InputLabel>
                                <Input value={self.state.newScope.description} name="description" onChange={self.handleChangeScopeProperty}  />
                                <FormHelperText>{t('scopeDescriptionDescription')}</FormHelperText>
                            </FormControl>
                        </DialogContent>
                        <DialogActions>
                            <Button  variant="raised" color="primary" onClick={self.handleAddScope}>{t('addScope')}</Button>
                        </DialogActions>
                    </div>
                ) }
            </Dialog>
            <div className="block-header">
                <Grid container>
                    <Grid item md={5} sm={12}>
                        <h4>{t('scopes')}</h4>
                        <i>{t('scopesDescription')}</i>
                    </Grid>
                    <Grid item md={7} sm={12}>                        
                        <ul className="breadcrumb float-md-right">
                            <li className="breadcrumb-item"><NavLink to="/">{t('websiteTitle')}</NavLink></li>
                            <li className="breadcrumb-item">{t('scopes')}</li>
                        </ul>
                    </Grid>
                </Grid>
            </div>
            <div className="card">
                <div className="header">
                    <h4 style={{display: "inline-block"}}>{t('listOfScopes')}</h4>
                    <div style={{float: "right"}}>
                        {self.state.isRemoveDisplayed && (
                            <IconButton onClick={self.handleRemoveScopes}>
                                <Delete />
                            </IconButton>
                        )}
                        <IconButton onClick={this.handleClick}>
                            <MoreVert />
                        </IconButton>
                        <Menu anchorEl={self.state.anchorEl} open={Boolean(self.state.anchorEl)} onClose={self.handleClose}>
                            <MenuItem onClick={self.handleOpenModal}>{t('addScope')}</MenuItem>
                        </Menu>
                    </div>
                </div>
                <div className="body">
                    { this.state.isLoading ? (<CircularProgress />) : (
                        <div>
                            <Table>
                                <TableHead>
                                    <TableRow>
                                        <TableCell></TableCell>
                                        <TableCell>{t('name')}</TableCell>
                                        <TableCell>{t('scopeType')}</TableCell>
                                        <TableCell>
                                            <TableSortLabel active={true} direction={self.state.order} onClick={self.handleSort}>{t('updateDateTime')}</TableSortLabel>
                                        </TableCell>
                                        <TableCell></TableCell>
                                    </TableRow>
                                </TableHead>
                                <TableBody>
                                    <TableRow>
                                        <TableCell><Checkbox color="primary" onChange={self.handleAllSelections} /></TableCell>
                                        <TableCell>
                                            <form onSubmit={(e) => { e.preventDefault(); self.refreshData(); }}>
                                                <TextField value={this.state.selectedName} name='selectedName' onChange={self.handleChangeProperty} placeholder={t('Filter...')}/>
                                                <IconButton onClick={self.refreshData}><Search /></IconButton>
                                            </form>
                                        </TableCell>
                                        <TableCell>
                                            <Select value={this.state.selectedType} fullWidth={true} name="selectedType" onChange={this.handleChangeFilter}>
                                                <MenuItem value="all">{t('all')}</MenuItem>
                                                <MenuItem value="0">{t('apiScope')}</MenuItem>
                                                <MenuItem value="1">{t('openidScope')}</MenuItem>
                                            </Select>
                                        </TableCell>                                        
                                        <TableCell></TableCell>                               
                                        <TableCell></TableCell>
                                    </TableRow>
                                    {rows}
                                </TableBody>
                                <TableFooter>
                                </TableFooter>
                            </Table>
                            <TablePagination component="div" count={self.state.count} rowsPerPage={self.state.pageSize} page={this.state.page} onChangePage={self.handleChangePage} onChangeRowsPerPage={self.handleChangeRowsPage} />
                        </div>
                    )}
                </div>
            </div>
        </div>);
    }

    componentDidMount() {
        var self = this;
        SessionStore.addChangeListener(function() {
        	self.setState({
        		type: self.props.type
        	}, function() {
        		self.refreshData();
        	});
        });
        if (SessionStore.getSession().selectedOpenid) {
        	self.setState({
        		type: self.props.type
        	}, function() {
        		self.refreshData();
        	});
        }
    }
}

export default translate('common', { wait: process && !process.release })(withStyles(styles)(withRouter(ViewScopes)));