import React, { Component } from "react";
import { translate } from 'react-i18next';
import { Grid } from 'material-ui';
import { NavLink } from 'react-router-dom';
import { IconButton, Menu, MenuItem, Button, Checkbox, CircularProgress, Hidden, List, ListItem, ListItemText } from 'material-ui';
import Table, { TableBody, TableCell, TableHead, TableRow, TableFooter } from 'material-ui/Table';
import Dialog, { DialogTitle, DialogContent, DialogActions } from 'material-ui/Dialog';
import { FormControl, FormHelperText } from 'material-ui/Form';
import Input, { InputLabel } from 'material-ui/Input';
import MoreVert from '@material-ui/icons/MoreVert';
import Delete from '@material-ui/icons/Delete';
import Visibility from '@material-ui/icons/Visibility'; 
import { SessionStore } from '../stores';
import { AccountFilterService } from '../services';
import AppDispatcher from '../appDispatcher';
import Constants from '../constants';

class AccountFilters extends Component {
    constructor(props) {
        super(props);
        this.handleRemoveAccountFilters = this.handleRemoveAccountFilters.bind(this);
        this.handleRemoveAccountFilter = this.handleRemoveAccountFilter.bind(this);
        this.handleRowClick = this.handleRowClick.bind(this);
        this.handleChangeProperty = this.handleChangeProperty.bind(this);        
        this.handleCloseModal = this.handleCloseModal.bind(this);
        this.handleClick = this.handleClick.bind(this);
        this.refreshData = this.refreshData.bind(this);
        this.handleClose = this.handleClose.bind(this);
        this.handleOpenModal = this.handleOpenModal.bind(this);
        this.handleAddAccountFilter = this.handleAddAccountFilter.bind(this);
        this.handleAllSelections = this.handleAllSelections.bind(this);
        this.state = {
            isRemoveDisplayed: false,
            isLoading: true,
            anchorEl: null,
            isModalOpened: false,
            isAddAccountFilterLoading: false,
            accountFilterName: null,
            accountFilters: []
        };
    }  


    /**
    * Remove the account filter.
    */
    handleRemoveAccountFilter(accountFilterId) {      
        var self = this;
        const { t } = self.props;
        self.setState({
            isLoading: true
        });
        AccountFilterService.deleteAccountFilter(accountFilterId).then(function() {
            AppDispatcher.dispatch({
                actionName: Constants.events.DISPLAY_MESSAGE,
                data: t('accountFilterRemoved')
            });
            self.refreshData();            
        }).catch(function() {
            AppDispatcher.dispatch({
                actionName: Constants.events.DISPLAY_MESSAGE,
                data: t('accountFilterCannotBeRemoved')
            });
            self.refreshData();
        });
    }  

    /**
    * Remove the selected account filters.
    */
    handleRemoveAccountFilters() {      
        var self = this;
        var accountFilterIds = self.state.accountFilters.filter(function(s) { return s.isSelected; }).map(function(s) { return s.id; });
        if (accountFilterIds.length === 0) {
            return;
        }

        const { t } = self.props;
        self.setState({
            isLoading: true
        });
        var operations = [];
        accountFilterIds.forEach(function(accountFilterId) {
            operations.push(AccountFilterService.deleteAccountFilter(accountFilterId));
        });

        Promise.all(operations).then(function() {
            AppDispatcher.dispatch({
                actionName: Constants.events.DISPLAY_MESSAGE,
                data: t('accountFiltersAreRemoved')
            });
            self.refreshData();            
        }).catch(function() {
            AppDispatcher.dispatch({
                actionName: Constants.events.DISPLAY_MESSAGE,
                data: t('accountFiltersCannotBeRemoved')
            });
            self.refreshData();
        });
    }  

    /**
    * Handle click on the row.
    */
    handleRowClick(e, record) {
        record.isSelected = e.target.checked;
        var data = this.state.accountFilters;
        var nbSelectedRecords = data.filter(function(r) { return r.isSelected; }).length;
        this.setState({
            accountFilters: data,
            isRemoveDisplayed: nbSelectedRecords > 0
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
    * Handle the changes.
    */
    handleChangeProperty(e) {
        var self = this;
        self.setState({
            [e.target.name]: e.target.value
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
    * Refresh the data.
    */
    refreshData() {
        var self  = this;
        self.setState({
            isLoading: true
        })
        AccountFilterService.getAllFilters().then(function(result) {
            result.forEach(function(record) {
                record.isSelected = false;
            });
            self.setState({
                isLoading: false,
                accountFilters: result
            });
        }).catch(function() {
            self.setState({
                isLoading: false,
                accountFilters: []
            });
            AppDispatcher.dispatch({
                actionName: Constants.events.DISPLAY_MESSAGE,
                data: t('accountFiltersCannotBeRetrieved')
            });
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
    * Open the modal.
    */
    handleOpenModal() {
        this.setState({
            isModalOpened: true
        });
    }

    /**
    * Add an account filter.
    */
    handleAddAccountFilter() {
        var self = this;
        const { t } = self.props;
        self.setState({
            isAddAccountFilterLoading: true
        });
        var request = {
            name: self.state.accountFilterName
        };
        AccountFilterService.addAccountFilter(request).then(function() {
            AppDispatcher.dispatch({
                actionName: Constants.events.DISPLAY_MESSAGE,
                data: t('accountFilterHasBeenAdded')
            });
            self.setState({
                isModalOpened: false,
                isAddAccountFilterLoading: false
            });
            self.refreshData();
        }).catch(function() {
            AppDispatcher.dispatch({
                actionName: Constants.events.DISPLAY_MESSAGE,
                data: t('cannotAddAccountFilter')
            });
            self.setState({
                isAddAccountFilterLoading: false
            });
        });
    }

    /**
    * Select / Unselect all account filters.
    */
    handleAllSelections(e) {
        var checked = e.target.checked;
        var data = this.state.accountFilters;
        data.forEach(function(r) { r.isSelected = checked ;});
        this.setState({
            accountFilters: data,
            isRemoveDisplayed: checked
        });
    }

    render() {
        var self = this;
        const { t } = self.props;
        var rows = [];
        var listItems = [];
        if (self.state.accountFilters) {
            self.state.accountFilters.forEach(function(accountFilter) {
                rows.push(
                    <TableRow>
                        <TableCell><Checkbox color="primary" checked={accountFilter.isSelected} onChange={(e) => self.handleRowClick(e, accountFilter)} /></TableCell>
                        <TableCell>{accountFilter.id}</TableCell>
                        <TableCell>{accountFilter.name}</TableCell>
                        <TableCell>
                            <NavLink to={'/openid/accountfilters/' + accountFilter.id}>
                                <IconButton>
                                    <Visibility />
                                </IconButton>
                            </NavLink>
                        </TableCell>
                    </TableRow>
                );                
                listItems.push(
                    <ListItem dense button style={{overflow: 'hidden'}}>
                        <IconButton onClick={() => self.handleRemoveAccountFilter(accountFilter.id)}>
                            <Delete />
                        </IconButton>
                        <NavLink to={'/openid/accountfilters/' + accountFilter.id }>
                            <IconButton>
                                <Visibility />
                            </IconButton>
                        </NavLink>
                        <ListItemText>{accountFilter.name}</ListItemText>
                    </ListItem>
                );
            });
        }

        return (<div className="block">
            <Dialog open={self.state.isModalOpened} onClose={this.handleCloseModal}>
                <DialogTitle>{t('addAccountFilter')}</DialogTitle>
                { self.state.isAddAccountFilterLoading ? (<DialogContent><CircularProgress /></DialogContent>)  : (
                    <div>
                        <DialogContent>
                            {/* Name */}
                            <FormControl fullWidth={true} style={{margin: "5px"}}>
                                <InputLabel>{t('accountFilterName')}</InputLabel>
                                <Input value={self.state.accountFilterName} name="accountFilterName" onChange={self.handleChangeProperty}  />
                                <FormHelperText>{t('accountFilterNameDescription')}</FormHelperText>
                            </FormControl>
                        </DialogContent>
                        <DialogActions>
                            <Button variant="raised" color="primary" onClick={self.handleAddAccountFilter}>{t('addAccountFilter')}</Button>
                        </DialogActions>
                    </div>
                )}
            </Dialog>
            <div className="block-header">
                <Grid container>
                    <Grid item md={5} sm={12}>
                        <h4>{t('accountFiltersTitle')}</h4>
                        <i>{t('accountFiltersDescription')}</i>
                    </Grid>
                    <Grid item md={7} sm={12}>                        
                        <ul className="breadcrumb float-md-right">
                            <li className="breadcrumb-item"><NavLink to="/">{t('websiteTitle')}</NavLink></li>
                            <li className="breadcrumb-item">{t('accountFilters')}</li>
                        </ul>
                    </Grid>
                </Grid>
            </div>
            <div className="card">
                <div className="header">
                    <h4 style={{display: "inline-block"}}>{t('listOfAccountFilters')}</h4>
                    <div style={{float: "right"}}>
                        { self.state.isRemoveDisplayed && (
                            <IconButton onClick={self.handleRemoveAccountFilters}>
                                <Delete />
                            </IconButton>
                        )}
                        <IconButton onClick={this.handleClick}>
                            <MoreVert />
                        </IconButton>
                        <Menu anchorEl={self.state.anchorEl} open={Boolean(self.state.anchorEl)} onClose={self.handleClose}>
                            <MenuItem onClick={self.handleOpenModal}>{t('addAccountFilter')}</MenuItem>
                        </Menu>
                    </div>
                </div>
                <div className="body">
                    {self.state.isLoading ? (<CircularProgress />) : (
                        <div>
                            <Hidden only={['xs', 'sm']}>
                                <Table>
                                    <TableHead>
                                        <TableRow>
                                            <TableCell><Checkbox color="primary" onChange={self.handleAllSelections} /></TableCell>
                                            <TableCell>{t('accountFilterId')}</TableCell>
                                            <TableCell>{t('accountFilterName')}</TableCell>
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

export default translate('common', { wait: process && !process.release })(AccountFilters);