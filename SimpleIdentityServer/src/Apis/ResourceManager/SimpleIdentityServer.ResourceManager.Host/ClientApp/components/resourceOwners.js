import React, { Component } from "react";
import { translate } from 'react-i18next';
import { ResourceOwnerService } from '../services';
import { NavLink, withRouter } from "react-router-dom";
import { SessionStore } from '../stores';
import moment from 'moment';

import Table, { TableBody, TableCell, TableHead, TableRow, TableFooter, TablePagination, TableSortLabel } from 'material-ui/Table';
import { Popover, IconButton, Menu, MenuItem, Checkbox, TextField, Select, Avatar , CircularProgress, Grid, Tooltip, Button, Hidden, List, ListItem, ListItemText } from 'material-ui';
import Dialog, { DialogTitle, DialogContent, DialogActions } from 'material-ui/Dialog';
import Input, { InputLabel } from 'material-ui/Input';
import { FormControl, FormHelperText } from 'material-ui/Form';
import MoreVert from '@material-ui/icons/MoreVert';
import Delete from '@material-ui/icons/Delete';
import Search from '@material-ui/icons/Search';
import Link from '@material-ui/icons/Link';
import Visibility from '@material-ui/icons/Visibility'; 
import AppDispatcher from '../appDispatcher';
import Constants from '../constants';

class ResourceOwners extends Component {
    constructor(props) {
        super(props);
        this.handleClick = this.handleClick.bind(this);
        this.handleClose = this.handleClose.bind(this);
        this.handleChangeFilter = this.handleChangeFilter.bind(this);
        this.refreshData = this.refreshData.bind(this);
        this.handleChangePage = this.handleChangePage.bind(this);
        this.handleChangeRowsPage = this.handleChangeRowsPage.bind(this);
        this.handleRowClick = this.handleRowClick.bind(this);
        this.handleAllSelections = this.handleAllSelections.bind(this);
        this.handleRemoveUsers = this.handleRemoveUsers.bind(this);
        this.handleRemoveUser = this.handleRemoveUser.bind(this);
        this.handleSort = this.handleSort.bind(this);
        this.handleCloseModal = this.handleCloseModal.bind(this);
        this.handleOpenModal = this.handleOpenModal.bind(this);
        this.handleAddUser = this.handleAddUser.bind(this);
        this.handleChangeNewUser = this.handleChangeNewUser.bind(this);
        this.state = {
            data: [],
            isLoading: false,
            page: 0,
            pageSize: 5,
            count: 0,
            anchorEl: null,
            isRemoveDisplayed: false,
            selectedSubject: '',
            order: 'desc',
            isModalOpened: false,
            isAddUserLoading: false,
            newUser: {}
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
    * Handle the change
    */
    handleChangeFilter(e) {
        var self = this;
        self.setState({
            [e.target.name]: e.target.value
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
        if (self.state.selectedSubject && self.state.selectedSubject !== '') {
            request['subjects'] = [ self.state.selectedSubject ];
        }

        var getClaim = function(claimName, claims, defaultValue) {
            for(var i in claims) {
                var claim = claims[i];
                if (claim.key === claimName) {
                    return claim.value;
                }
            }

            return defaultValue;
        };

        ResourceOwnerService.search(request, self.props.type).then(function (result) {
            var data = [];
            if (result.content) {
                result.content.forEach(function (client) {
                    var record = {
                        login: client['login'],
                        isSelected: false,
                        picture: getClaim("picture", client.claims, ""),
                        email: getClaim("email", client.claims, "-"),
                        name: getClaim("given_name", client.claims, "-"),
                        scimId: getClaim("scim_id", client.claims, ""),
                        update_datetime: client['update_datetime']
                    };

                    data.push(record);
                });
            }

            self.setState({
                isLoading: false,
                data: data,
                pageSize: self.state.pageSize,
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
        self.setState({
            page: page
        }, function() {
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
        }, function() {
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
    * Remove the selected users.
    */
    handleRemoveUsers() {      
        var self = this;
        const {t} = self.props;
        var users = self.state.data.filter(function(user) { return user.isSelected; }).map(function(user) { return user.login; });
        if (users.length === 0) {
            return;
        }

        self.setState({
            isLoading: true
        });
        var ops = [];
        users.forEach(function(user) {
            ops.push(ResourceOwnerService.delete(user));
        });
        Promise.all(ops).then(function() {
            AppDispatcher.dispatch({
                actionName: Constants.events.DISPLAY_MESSAGE,
                data: t('usersAreRemoved')
            });
            self.refreshData();
        }).catch(function() {
            self.setState({
                isLoading: false
            });
            AppDispatcher.dispatch({
                actionName: Constants.events.DISPLAY_MESSAGE,
                data: t('usersCannotBeRemoved')
            });
        });
    }

    /**
    * Remove the user
    */
    handleRemoveUser(login) {
        var self = this;
        const {t} = self.props;
        self.setState({
            isLoading: true
        });
        ResourceOwnerService.delete(login).then(function() {
            AppDispatcher.dispatch({
                actionName: Constants.events.DISPLAY_MESSAGE,
                data: t('userIsRemoved')
            });
            self.refreshData();
        }).catch(function() {
            self.setState({
                isLoading: false
            });
            AppDispatcher.dispatch({
                actionName: Constants.events.DISPLAY_MESSAGE,
                data: t('userCannotBeRemoved')
            });
        });
    }

    /**
    * Sort the users.
    */
    handleSort() {
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
    * Add a user.
    */
    handleAddUser() {
        var self = this;
        self.setState({
            isAddUserLoading: true
        });
        const { t } = self.props;
        ResourceOwnerService.add(self.state.newUser).then(function() {
            self.setState({
                isAddUserLoading: false,
                isModalOpened: false,
                newUser: {}
            });
            AppDispatcher.dispatch({
                actionName: Constants.events.DISPLAY_MESSAGE,
                data: t('userIsAdded')
            });
            self.refreshData();
        }).catch(function(e) {
            self.setState({
                isAddUserLoading: false
            });
            AppDispatcher.dispatch({
                actionName: Constants.events.DISPLAY_MESSAGE,
                data: t('userCannotBeAdded')
            });
        });
    }

    /**
    * Change thbe user information.
    */
    handleChangeNewUser(e) {
        var newUser = this.state.newUser;
        newUser[e.target.name] = e.target.value;
        this.setState({
            newUser: newUser
        });
    }

    render() {
        var self = this;
        const { t } = this.props;
        var rows = [], listItems = [];
        if (self.state.data) {
            self.state.data.forEach(function(record) {
                rows.push((
                    <TableRow hover role="checkbox" key={record.login}>
                        <TableCell><Checkbox color="primary" checked={record.isSelected} onChange={(e) => self.handleRowClick(e, record)} /></TableCell>
                        <TableCell><Avatar src={record.picture}/></TableCell>
                        <TableCell>{record.login}</TableCell>
                        <TableCell>{record.email}</TableCell>
                        <TableCell>{record.name}</TableCell>
                        <TableCell>{moment(record.update_datetime).format('LLLL')}</TableCell>
                        <TableCell>
                            { record.scimId !== "" && 
                                <NavLink to={'/scim/resources/user/' + record.scimId}>
                                    <Tooltip title={t('viewScimResource')}>
                                        <IconButton>
                                            <Link />
                                        </IconButton>
                                    </Tooltip>
                                </NavLink>
                            }
                            <NavLink to={'/users/' + record.login}>
                                <IconButton>
                                    <Visibility />
                                </IconButton>
                            </NavLink>
                        </TableCell>
                    </TableRow>
                ));
                listItems.push(                    
                    <ListItem dense button style={{overflow: 'hidden'}}>
                        <IconButton onClick={() => self.handleRemoveUser(record.login)}>
                            <Delete />
                        </IconButton>
                        <NavLink to={'/users/' + record.login }>
                            <IconButton>
                                <Visibility />
                            </IconButton>
                        </NavLink>
                        <ListItemText>{record.login}</ListItemText>
                    </ListItem>
                );
            });
        }

        return (<div className="block">
            <Dialog open={self.state.isModalOpened} onClose={this.handleCloseModal}>
                <DialogTitle>{t('addUser')}</DialogTitle>
                {self.state.isAddUserLoading ? (<DialogContent><CircularProgress /></DialogContent>) : (
                    <form onSubmit={(e) => { e.preventDefault(); self.handleAddUser(); }}>
                        <DialogContent>
                            {/* Login */}
                            <FormControl fullWidth={true}>
                                <InputLabel>{t('userLogin')}</InputLabel>
                                <Input value={self.state.newUser.sub} name="sub" onChange={self.handleChangeNewUser}  />
                                <FormHelperText>{t('userLoginShortDescription')}</FormHelperText>
                            </FormControl>
                            {/* Password */}
                            <FormControl fullWidth={true}>
                                <InputLabel>{t('userPassword')}</InputLabel>
                                <Input type="password" value={self.state.newUser.password} name="password" onChange={self.handleChangeNewUser}  />
                                <FormHelperText>{t('userPasswordShortDescription')}</FormHelperText>
                            </FormControl>
                        </DialogContent>
                        <DialogActions>
                            <Button  variant="raised" color="primary" onClick={self.handleAddUser}>{t('addUser')}</Button>
                        </DialogActions>
                    </form>
                )}
            </Dialog>
            <div className="block-header">
                <Grid container>
                    <Grid item md={5} sm={12}>
                        <h4>{t('users')}</h4>
                        <i>{t('usersShortDescription')}</i>
                    </Grid>
                    <Grid item md={7} sm={12}>
                        <ul className="breadcrumb float-md-right">
                            <li className="breadcrumb-item"><NavLink to="/">{t('websiteTitle')}</NavLink></li>
                            <li className="breadcrumb-item">{t('resourceOwners')}</li>
                        </ul>
                    </Grid>
                </Grid>
            </div>
            <div className="card">
                <div className="header">
                    <h4 style={{display: "inline-block"}}>{t('listOfUsers')}</h4>
                    <div style={{float: "right"}}>
                        {self.state.isRemoveDisplayed && (
                            <IconButton onClick={self.handleRemoveUsers}>
                                <Delete />
                            </IconButton>
                        )}
                        <IconButton onClick={this.handleClick}>
                            <MoreVert />
                        </IconButton>
                        <Menu anchorEl={self.state.anchorEl} open={Boolean(self.state.anchorEl)} onClose={self.handleClose}>
                            <MenuItem onClick={self.handleOpenModal}>{t('addUser')}</MenuItem>
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
                                            <TableCell>
                                                {t('userSubject')}                                            
                                                <form onSubmit={(e) => { e.preventDefault(); self.refreshData(); }}>
                                                    <TextField value={this.state.selectedSubject} name='selectedSubject' onChange={this.handleChangeFilter} placeholder={t('Filter...')}/>
                                                    <IconButton onClick={self.refreshData}><Search /></IconButton>
                                                </form>
                                            </TableCell>
                                            <TableCell>{t('userEmail')}</TableCell>
                                            <TableCell>{t('userName')}</TableCell>
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
            self.refreshData();
        });
        if (SessionStore.getSession().selectedOpenid) {
            self.refreshData();
        }
    }
}

export default translate('common', { wait: process && !process.release })(withRouter(ResourceOwners));