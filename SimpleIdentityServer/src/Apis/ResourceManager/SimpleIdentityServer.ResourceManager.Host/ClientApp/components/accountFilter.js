import React, { Component } from "react";
import { translate } from 'react-i18next';
import { Grid } from 'material-ui';
import { NavLink } from 'react-router-dom';
import { IconButton, Menu, MenuItem, Button, Checkbox, CircularProgress } from 'material-ui';
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

class AccountFilter extends Component {
    constructor(props) {
        super(props);
        this.handleChangeProperty = this.handleChangeProperty.bind(this);        
        this.handleCloseModal = this.handleCloseModal.bind(this);
        this.handleClick = this.handleClick.bind(this);
        this.refreshData = this.refreshData.bind(this);
        this.handleClose = this.handleClose.bind(this);
        this.handleOpenModal = this.handleOpenModal.bind(this);
        this.handleAddAccountFilter = this.handleAddAccountFilter.bind(this);
        this.handleAllSelections = this.handleAllSelections.bind(this);
        this.state = {
            isLoading: true,
            anchorEl: null,
            isModalOpened: false,
            isAddAccountFilterLoading: false,
            accountFilterName: null,
            accountFilters: []
        };
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
            self.setState({
                isLoading: false
            });
            AppDispatcher.dispatch({
                actionName: Constants.events.DISPLAY_MESSAGE,
                data: t('accountFilterHasBeenAdded')
            });
        }).catch(function() {
            self.setState({
                isLoading: false
            });
            AppDispatcher.dispatch({
                actionName: Constants.events.DISPLAY_MESSAGE,
                data: t('cannotAddAccountFilter')
            });
        });
    }

    /**
    * Select all the line.
    */
    handleAllSelections() {

    }

    render() {
        var self = this;
        const { t } = self.props;
        var rows = [];
        if (self.state.accountFilters) {
            self.state.accountFilters.forEach(function(accountFilter) {
                rows.push(
                    <TableRow>
                        <TableCell><Checkbox color="primary" /></TableCell>
                        <TableCell>{accountFilter.id}</TableCell>
                        <TableCell>{accountFilter.name}</TableCell>
                    </TableRow>
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
                        <h4>{t('accountFilterTitle')}</h4>
                        <i>{t('accountFilterDescription')}</i>
                    </Grid>
                    <Grid item md={7} sm={12}>                        
                        <ul className="breadcrumb float-md-right">
                            <li className="breadcrumb-item"><NavLink to="/">{t('websiteTitle')}</NavLink></li>
                            <li className="breadcrumb-item">{t('accountFiltering')}</li>
                        </ul>
                    </Grid>
                </Grid>
            </div>
            <div className="card">
                <div className="header">
                    <h4 style={{display: "inline-block"}}>{t('listOfAccountFilters')}</h4>
                    <div style={{float: "right"}}>
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
                        <Table>
                            <TableHead>
                                <TableRow>
                                    <TableCell><Checkbox color="primary" onChange={self.handleAllSelections} /></TableCell>
                                    <TableCell>{t('accountFilterId')}</TableCell>
                                    <TableCell>{t('accountFilterName')}</TableCell>
                                </TableRow>
                            </TableHead>
                            <TableBody>
                                {rows}
                            </TableBody>
                        </Table>
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

export default translate('common', { wait: process && !process.release })(AccountFilter);