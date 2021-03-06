import React, { Component } from "react";
import { translate } from 'react-i18next';
import { ResourceService } from '../../services';
import { NavLink, withRouter } from 'react-router-dom';
import { ChipsSelector } from '../common';

import Table, { TableBody, TableCell, TableHead, TableRow, TableFooter, TablePagination } from 'material-ui/Table';
import { Popover, Hidden, IconButton, Menu, MenuItem, Checkbox, TextField, Select, Avatar, CircularProgress, List, ListItem, Button, ListItemText } from 'material-ui';
import Dialog, { DialogTitle, DialogContent, DialogActions } from 'material-ui/Dialog';
import Input, { InputLabel } from 'material-ui/Input';
import { FormControl, FormHelperText } from 'material-ui/Form';
import Delete from '@material-ui/icons/Delete';
import MoreVert from '@material-ui/icons/MoreVert';
import Visibility from '@material-ui/icons/Visibility';
import AppDispatcher from '../../appDispatcher';
import Constants from '../../constants';
import { SessionStore } from '../../stores';

class Resources extends Component {
    constructor(props) {
        super(props);
        this.handleClick = this.handleClick.bind(this);
        this.handleClose = this.handleClose.bind(this);
        this.handleAllSelections = this.handleAllSelections.bind(this);
        this.handleRowClick = this.handleRowClick.bind(this);
        this.refreshData = this.refreshData.bind(this);
        this.handleRemoveResources = this.handleRemoveResources.bind(this);
        this.handleRemoveResource = this.handleRemoveResource.bind(this);
        this.handleCloseModal = this.handleCloseModal.bind(this);
        this.handleOpenModal = this.handleOpenModal.bind(this);
        this.handleAddResource = this.handleAddResource.bind(this);
        this.handleChangeResource = this.handleChangeResource.bind(this);
        this.handleChangePage = this.handleChangePage.bind(this);
        this.handleChangeRowsPage = this.handleChangeRowsPage.bind(this);
        this.state = {
            isLoading: false,
            page: 0,
            pageSize: 5,
            isRemoveDisplayed: false,
            anchorEl: null,
            isModalOpened: false,
            isAddResourceLoading: false,
            newResource: {
                scopes: []
            }
        };
    }

    /**
    * Change the resource information.
    */
    handleChangeResource(e) {
        var newResource = this.state.newResource;
        newResource[e.target.name] = e.target.value;
        this.setState({
            newResource: newResource
        });
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
    * Add the resource.
    */
    handleAddResource() {
        var self = this;
        const {t} = self.props;
        self.setState({
            isAddResourceLoading: true
        });
        ResourceService.add(self.state.newResource).then(function() {
            self.setState({
                isAddResourceLoading: false,
                isModalOpened: false
            });
            AppDispatcher.dispatch({
                actionName: Constants.events.DISPLAY_MESSAGE,
                data: t('resourceAdded')
            });
            self.refreshData();
        }).catch(function() {
            AppDispatcher.dispatch({
                actionName: Constants.events.DISPLAY_MESSAGE,
                data: t('resourceCannotBeAdded')
            });
            self.setState({
                isAddResourceLoading: false
            });
        });
    }

    /**
    * Select all the data.
    */
    handleAllSelections(e) {
        var self = this;
        var checked = e.target.checked;
        var data = self.state.data;
        data.forEach(function(r) { r.isSelected = checked ;});
        self.setState({
            data: data,
            isRemoveDisplayed: checked
        });
    }

    /**
    * Handle click on the row.
    */
    handleRowClick(e, record) {
        var self  = this;
        record.isSelected = e.target.checked;
        var data = self.state.data;
        var nbSelectedRecords = data.filter(function(r) { return r.isSelected; }).length;
        self.setState({
            data: data,
            isRemoveDisplayed: nbSelectedRecords > 0
        });
    }

    /**
    * Refresh the resources.
    */
    refreshData() {
        var self = this;
        var startIndex = self.state.page * self.state.pageSize;
        self.setState({
            isLoading: true
        });

        var request = { start_index: startIndex, count: self.state.pageSize };
        ResourceService.search(request, self.props.type).then(function (result) {
            var data = [];
            if (result.content) {
                result.content.forEach(function (r) {
                    var scopes = r['scopes'] ? r['scopes'].join(',') : '-';
                    data.push({
                        id: r['_id'],
                        name: r['name'],
                        type: r['type'],
                        scopes: scopes,
                        isSelected: false
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
                data: []
            });
        });
    }

    /**
    * Remove the selected resources.
    */
    handleRemoveResources() {
        var self = this;
        var resourceIds = self.state.data.filter(function(d) { return d.isSelected; }).map(function(d) { return d.id; });
        if(resourceIds.length === 0) {
            return;
        }

        const { t } = self.props;
        self.setState({
            isLoading: true
        });
        var operations = [];
        resourceIds.forEach(function(resourceId) {
            operations.push(ResourceService.delete(resourceId));
        });
        Promise.all(operations).then(function() {
            AppDispatcher.dispatch({
                actionName: Constants.events.DISPLAY_MESSAGE,
                data: t('resourcesAreRemoved')
            });
            self.setState({
                isRemoveDisplayed: false
            });
            self.refreshData();
        }).catch(function(e) {
            self.setState({
                isLoading: false
            });
            AppDispatcher.dispatch({
                actionName: Constants.events.DISPLAY_MESSAGE,
                data: t('resourcesCannotBeRemoved')
            });
        });
    }

    /**
    * Remove the resource.
    */
    handleRemoveResource(resourceId) {
        var self = this;
        const { t } = self.props;
        self.setState({
            isLoading: true
        });
        ResourceService.delete(resourceId).then(function() {
            AppDispatcher.dispatch({
                actionName: Constants.events.DISPLAY_MESSAGE,
                data: t('resourceIsRemoved')
            });
            self.setState({
                isRemoveDisplayed: false
            });
            self.refreshData();
        }).catch(function(e) {
            self.setState({
                isLoading: false
            });
            AppDispatcher.dispatch({
                actionName: Constants.events.DISPLAY_MESSAGE,
                data: t('resourceCannotBeRemoved')
            });
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
            isModalOpened: true,
            newResource: {
                scopes: []
            }
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

    render() {
        var self = this;
        const { t } = self.props;
        var rows = [], listItems = [];
        if (self.state.data) {
            self.state.data.forEach(function(record) {
                rows.push(
                    <TableRow key={record.id}>
                        <TableCell><Checkbox color="primary" checked={record.isSelected} onChange={(e) => self.handleRowClick(e, record)} /></TableCell>
                        <TableCell>{record.id}</TableCell>
                        <TableCell>{record.type}</TableCell>
                        <TableCell>{record.scopes}</TableCell>
                        <TableCell>
                            <IconButton onClick={ () => self.props.history.push('/resources/' + record.id + '/edit') }><Visibility /></IconButton>
                        </TableCell>
                    </TableRow>
                );
                listItems.push(
                    <ListItem dense button style={{overflow: 'hidden'}}>
                        <IconButton onClick={() => self.handleRemoveResource(record.id)}>
                            <Delete />
                        </IconButton>
                        <NavLink to={'/resources/' + record.id + '/edit'}>
                            <IconButton>
                                <Visibility />
                            </IconButton>
                        </NavLink>
                        <ListItemText>{record.id} (scopes : {record.scopes})</ListItemText>
                    </ListItem>
                );
            });
        }

        return (<div>
                <Dialog open={self.state.isModalOpened} onClose={this.handleCloseModal}>
                    <DialogTitle>{t('addResource')}</DialogTitle>
                    {self.state.isAddResourceLoading ? (<CircularProgress />) : (
                        <div>
                            <DialogContent>
                                {/* Resource name */ }
                                <FormControl fullWidth={true}>
                                    <InputLabel htmlFor="provider">{t('resourceName')}</InputLabel>
                                    <Input id="provider" value={self.state.newResource.name} name="name" onChange={(e) => { self.handleChangeResource(e); }}  />
                                    <FormHelperText>{t('resourceNameDescription')}</FormHelperText>
                                </FormControl>
                                {/* Resource type */ }
                                <FormControl fullWidth={true}>
                                    <InputLabel htmlFor="provider">{t('resourceType')}</InputLabel>
                                    <Input id="provider" value={self.state.newResource.type} name="type" onChange={(e) => { self.handleChangeResource(e); }}  />
                                    <FormHelperText>{t('resourceTypeDescription')}</FormHelperText>
                                </FormControl>
                                {/* Resource scopes */}
                                <FormControl fullWidth={true}>
                                    <ChipsSelector label={t('resourceScopes')} properties={self.state.newResource.scopes} />
                                </FormControl>
                            </DialogContent>
                            <DialogActions>
                                <Button  variant="raised" color="primary" onClick={self.handleAddResource}>{t('addResource')}</Button>
                            </DialogActions>
                        </div>
                    )}
                </Dialog>
                <div className="card">
                    <div className="header">
                        <h4 style={{display: "inline-block"}}>{t('resources')}</h4>
                        <div style={{float: "right"}}>
                            {self.state.isRemoveDisplayed && (
                                <IconButton onClick={self.handleRemoveResources}>
                                    <Delete />
                                </IconButton>
                            )}
                            <IconButton onClick={this.handleClick}>
                                <MoreVert />
                            </IconButton>
                            <Menu anchorEl={self.state.anchorEl} open={Boolean(self.state.anchorEl)} onClose={self.handleClose}>
                                <MenuItem onClick={self.handleOpenModal}>{t('addResource')}</MenuItem>
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
                                                <TableCell>{t('resourceId')}</TableCell>
                                                <TableCell>{t('resourceType')}</TableCell>
                                                <TableCell>{t('resourceScopes')}</TableCell>
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

export default translate('common', { wait: process && !process.release })(withRouter(Resources));