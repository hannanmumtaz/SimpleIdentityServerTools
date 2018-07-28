import React, { Component } from "react";
import { translate } from 'react-i18next';
import { withRouter, NavLink } from 'react-router-dom';
import ChipsSelector from './chipsSelector';
import moment from 'moment';

import Table, { TableBody, TableCell, TableHead, TableRow, TableFooter, TablePagination, TableSortLabel } from 'material-ui/Table';
import { Popover, IconButton, Menu, MenuItem, Checkbox, TextField, Select, Avatar, CircularProgress, Grid, Typography, Button, Hidden, List, ListItem, ListItemText } from 'material-ui';
import Dialog, { DialogTitle, DialogContent, DialogActions } from 'material-ui/Dialog';
import { withStyles } from 'material-ui/styles';
import { FormControl, FormHelperText } from 'material-ui/Form';
import MoreVert from '@material-ui/icons/MoreVert';
import Delete from '@material-ui/icons/Delete';
import Search from '@material-ui/icons/Search';
import Visibility from '@material-ui/icons/Visibility'; 
import { ClientService } from '../../services';
import { SessionStore } from '../../stores';
import AppDispatcher from '../../appDispatcher';
import Constants from '../../constants';

class Clients extends Component {
    constructor(props) {
        super(props);
        this.handleClick = this.handleClick.bind(this);
        this.handleClose = this.handleClose.bind(this);
        this.handleChangeValue = this.handleChangeValue.bind(this);
        this.handleChangeFilter = this.handleChangeFilter.bind(this);
        this.refreshData = this.refreshData.bind(this);
        this.handleChangePage = this.handleChangePage.bind(this);
        this.handleChangeRowsPage = this.handleChangeRowsPage.bind(this);
        this.handleRowClick = this.handleRowClick.bind(this);
        this.handleAllSelections = this.handleAllSelections.bind(this);
        this.handleRemoveClients = this.handleRemoveClients.bind(this);
        this.handleRemoveClient = this.handleRemoveClient.bind(this);
        this.handleSort = this.handleSort.bind(this);
        this.handleCloseModal = this.handleCloseModal.bind(this);
        this.handleOpenModal = this.handleOpenModal.bind(this);
        this.handleAddClient = this.handleAddClient.bind(this);

        this.state = {
        	type: '',
            data: [],
            isLoading: false,
            isAddClientLoading: false,
            page: 0,
            pageSize: 5,
            count: 0,
            anchorEl: null,
            isRemoveDisplayed: false,
            selectedId: null,
            selectedType: 'all',
            order: 'desc',
            isModalOpened: false,
            client: {
                redirect_uris: []
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
    handleChangeValue(e) {
        if (!e.target) { return; }
        var self = this;
        self.setState({
            [e.target.name]: e.target.value
        });
    }

    /**
    * Handle the change
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
    * Refresh the clients.
    */
    refreshData() { 
        var self = this;
        var startIndex = self.state.page * self.state.pageSize;
        self.setState({
            isLoading: true
        });

        var request = { start_index: startIndex, count: self.state.pageSize, order: { target: 'update_datetime', type: (self.state.order === 'desc' ? 1 : 0) } };
        if (self.state.selectedId && self.state.selectedId !== '') {
            request['client_ids'] = [ self.state.selectedId ];
        }

        if (self.state.selectedType !== 'all') {
            request['client_types'] = [ self.state.selectedType ];
        }

        ClientService.search(request, self.state.type).then(function (result) {
            var data = [];
            if (result.content) {
                result.content.forEach(function (client) {
                    data.push({
                        logo_uri: client['logo_uri'],
                        client_name: client['client_name'],
                        client_id: client['client_id'],
                        isSelected: false,
                        type: client['application_type'],
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
    * Remove the selected clients.
    */
    handleRemoveClients() {      
        var self = this;
        var clientIds = self.state.data.filter(function(s) { return s.isSelected; }).map(function(s) { return s.client_id; });
        if (clientIds.length === 0) {
            return;
        }

        const { t } = self.props;
        self.setState({
            isLoading: true
        });
        var operations = [];
        clientIds.forEach(function(clientId) {
            operations.push(ClientService.remove(clientId, self.state.type));
        });

        Promise.all(operations).then(function() {
            AppDispatcher.dispatch({
                actionName: Constants.events.DISPLAY_MESSAGE,
                data: t('clientsAreRemoved')
            });
            self.refreshData();
        }).catch(function(e) {
            self.setState({
                isLoading: false
            });
            AppDispatcher.dispatch({
                actionName: Constants.events.DISPLAY_MESSAGE,
                data: t('clientsCannotBeRemoved')
            });
        });
    }

    /**
    * Remove a client.
    */
    handleRemoveClient(clientId) {
        var self = this;
        const { t } = self.props;
        self.setState({
            isLoading: true
        });
        ClientService.remove(clientId, self.state.type).then(function() {
            AppDispatcher.dispatch({
                actionName: Constants.events.DISPLAY_MESSAGE,
                data: t('clientIsRemoved')
            });
            self.refreshData();
        }).catch(function(e) {
            self.setState({
                isLoading: false
            });
            AppDispatcher.dispatch({
                actionName: Constants.events.DISPLAY_MESSAGE,
                data: t('clientCannotBeRemoved')
            });
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
    * Add the client.
    */
    handleAddClient() {
        var self = this;
        self.setState({
            isAddClientLoading: true
        });
        const { t } = self.props;
        ClientService.add(self.state.client, self.state.type).then(function() {
            self.setState({
                isAddClientLoading: false,
                isModalOpened: false,
                client: {
                    redirect_uris:[]
                }
            });
            AppDispatcher.dispatch({
                actionName: Constants.events.DISPLAY_MESSAGE,
                data: t('clientIsAdded')
            });
            self.refreshData();
        }).catch(function() {
            self.setState({
                isAddClientLoading: false
            });
            AppDispatcher.dispatch({
                actionName: Constants.events.DISPLAY_MESSAGE,
                data: t('clientCannotBeAdded')
            });
        });
    }

    render() {
        var self = this;
        const { t, classes } = this.props;
        var rows = [], listItems = [];
        if (self.state.data) {
            self.state.data.forEach(function(record) {
                rows.push((
                    <TableRow hover role="checkbox" key={record.client_id}>
                        <TableCell><Checkbox color="primary" checked={record.isSelected} onChange={(e) => self.handleRowClick(e, record)} /></TableCell>
                        <TableCell><Avatar src={record.logo_uri}/></TableCell>
                        <TableCell>{record.client_name}</TableCell>
                        <TableCell>{record.client_id}</TableCell>
                        <TableCell>{record.type}</TableCell>
                        <TableCell>{moment(record.update_datetime).format('LLLL')}</TableCell>
                        <TableCell>
                            <IconButton onClick={ () => self.props.history.push('/' + self.state.type + '/clients/' + record.client_id) }><Visibility /></IconButton>
                        </TableCell>
                    </TableRow>
                ));
                listItems.push(
                    <ListItem dense button style={{overflow: 'hidden'}}>
                        <IconButton onClick={() => self.handleRemoveClient(record.client_id)}>
                            <Delete />
                        </IconButton>
                        <NavLink to={'/' + self.state.type + '/clients/' + record.client_id + '/edit'}>
                            <IconButton>
                                <Visibility />
                            </IconButton>
                        </NavLink>
                        <ListItemText>{record.client_id}</ListItemText>
                    </ListItem>
                )
            });
        }

        return (<div className="block">
            <Dialog open={self.state.isModalOpened} onClose={this.handleCloseModal}>
                <DialogTitle>{t('addClient')}</DialogTitle>
                {self.state.isAddClientLoading ? (<DialogContent><CircularProgress /></DialogContent>) : (
                    <div>
                        <DialogContent>
                            <ChipsSelector label={t('clientAllowedCallbackUrls')} properties={self.state.client.redirect_uris} />
                            <FormHelperText>{t('clientAllowedCallbackUrlsDescription')}</FormHelperText>
                        </DialogContent>
                        <DialogActions>
                            <Button  variant="raised" color="primary" onClick={self.handleAddClient}>{t('addClient')}</Button>
                        </DialogActions>
                    </div>
                )}
            </Dialog>
            <div className="block-header">
                <Grid container>
                    <Grid item md={5} sm={12}>
                        <h4>{t('oauthClients')}</h4>
                        <i>{t('oauthClientsShortDescription')}</i>
                    </Grid>
                    <Grid item md={7} sm={12}>                        
                        <ul className="breadcrumb float-md-right">
                            <li className="breadcrumb-item"><NavLink to="/">{t('websiteTitle')}</NavLink></li>
                            <li className="breadcrumb-item">{t('oauthClients')}</li>
                        </ul>
                    </Grid>
                </Grid>
            </div>
            <div className="card">
                <div className="header">
                    <h4 style={{display: "inline-block"}}>{t('listOfClients')}</h4>
                    <div style={{float: "right"}}>
                        {self.state.isRemoveDisplayed && (
                            <IconButton onClick={self.handleRemoveClients}>
                                <Delete />
                            </IconButton>
                        )}
                        <IconButton onClick={this.handleClick}>
                            <MoreVert />
                        </IconButton>
                        <Menu anchorEl={self.state.anchorEl} open={Boolean(self.state.anchorEl)} onClose={self.handleClose}>
                            <MenuItem onClick={self.handleOpenModal}>{t('addClient')}</MenuItem>
                        </Menu>
                    </div>
                </div>
                <div className="body">
                    { this.state.isLoading ? (<CircularProgress />) : (
                        <div>
                            <Hidden only={['xs', 'sm']}>
                                <Table>
                                    <TableHead>
                                        <TableRow>
                                            <TableCell><Checkbox color="primary" onChange={self.handleAllSelections} /></TableCell>
                                            <TableCell></TableCell>
                                            <TableCell>{t('clientName')}</TableCell>
                                            <TableCell>
                                                {t('clientId')}
                                                <form onSubmit={(e) => { e.preventDefault(); self.refreshData(); }}>
                                                    <TextField value={this.state.selectedId} name='selectedId' onChange={this.handleChangeValue} placeholder={t('Filter...')}/>
                                                    <IconButton onClick={self.refreshData}><Search /></IconButton>
                                                </form>
                                            </TableCell>
                                            <TableCell>
                                                {t('clientType')}                                                                                                                                               
                                                <Select value={this.state.selectedType} fullWidth={true} name="selectedType" onChange={this.handleChangeFilter}>
                                                    <MenuItem value="all">{t('all')}</MenuItem>
                                                    <MenuItem value="0">{t('native')}</MenuItem>
                                                    <MenuItem value="1">{t('web')}</MenuItem>
                                                </Select>
                                            </TableCell>
                                            <TableCell>
                                                <TableSortLabel active={true} direction={self.state.order} onClick={self.handleSort}>{t('updateDateTime')}</TableSortLabel>
                                            </TableCell>
                                            <TableCell></TableCell>
                                        </TableRow>
                                    </TableHead>
                                    <TableBody>
                                        {rows}
                                    </TableBody>
                                </Table>
                            </Hidden>
                            <Hidden only={['lg', 'xl', 'md']}>
                                <List>
                                    {listItems}
                                </List>
                            </Hidden>
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

export default translate('common', { wait: process && !process.release })(withRouter(Clients));