import React, { Component } from "react";
import { translate } from 'react-i18next';
import { ClaimService } from '../services';
import { NavLink, withRouter } from "react-router-dom";
import { SessionStore } from '../stores';
import moment from 'moment';

import Table, { TableBody, TableCell, TableHead, TableRow, TableFooter, TablePagination, TableSortLabel } from 'material-ui/Table';
import { Popover, IconButton, Menu, MenuItem, Checkbox, TextField, Select, Avatar , CircularProgress, Grid, Button } from 'material-ui';
import Dialog, { DialogTitle, DialogContent, DialogActions } from 'material-ui/Dialog';
import Input, { InputLabel } from 'material-ui/Input';
import { FormControl, FormHelperText } from 'material-ui/Form';
import MoreVert from '@material-ui/icons/MoreVert';
import Delete from '@material-ui/icons/Delete';
import Search from '@material-ui/icons/Search';
import Visibility from '@material-ui/icons/Visibility'; 
import AppDispatcher from '../appDispatcher';
import Constants from '../constants';

class Claims extends Component {
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
        this.handleRemoveClaims = this.handleRemoveClaims.bind(this);
        this.handleSort = this.handleSort.bind(this);
        this.handleCloseModal = this.handleCloseModal.bind(this);
        this.handleOpenModal = this.handleOpenModal.bind(this);
        this.handleAddClaim = this.handleAddClaim.bind(this);
        this.state = {
            data: [],
            isLoading: false,
            page: 0,
            pageSize: 5,
            count: 0,
            anchorEl: null,
            isRemoveDisplayed: false,
            selectedCode: '',
            order: 'desc',
            isModalOpened: false,
            isAddClaimLoading: false,
            newClaimCode: ''
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
        if (self.state.selectedCode && self.state.selectedCode !== '') {
            request['codes'] = [ self.state.selectedCode ];
        }

        ClaimService.search(request, self.props.type).then(function (result) {
            var data = [];
            if (result.content) {
                result.content.forEach(function (claim) {
                    var record = {
                        code: claim.key,
                        update_datetime: claim.update_datetime,
                        is_identifier: claim.is_identifier,
                        isSelected: false
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
    handleRemoveClaims() {      
        var self = this;
        var claimCodes = self.state.data.filter(function(s) { return s.isSelected; }).map(function(s) { return s.code; });
        if (claimCodes.length === 0) {
            return;
        }

        const { t } = self.props;
        self.setState({
            isLoading: true
        });
        var operations = [];
        claimCodes.forEach(function(claimCode) {
            operations.push(ClaimService.delete(claimCode));
        });

        Promise.all(operations).then(function() {
            AppDispatcher.dispatch({
                actionName: Constants.events.DISPLAY_MESSAGE,
                data: t('claimsAreRemoved')
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
                data: t('claimsCannotBeRemoved')
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
    * Add the claim.
    */
    handleAddClaim() {
        var self = this;
        self.setState({
            isAddClaimLoading: true
        });
        const { t } = self.props;
        ClaimService.add({key : self.state.newClaimCode}).then(function() {
            self.setState({
                isAddClaimLoading: false,
                isModalOpened: false,
                newClaimCode: ''
            });
            AppDispatcher.dispatch({
                actionName: Constants.events.DISPLAY_MESSAGE,
                data: t('claimIsAdded')
            });
            self.refreshData();
        }).catch(function() {
            self.setState({
                isAddClaimLoading: false
            });
            AppDispatcher.dispatch({
                actionName: Constants.events.DISPLAY_MESSAGE,
                data: t('claimCannotBeAdded')
            });
        });
    }

    render() {
        var self = this;
        const { t } = this.props;
        var rows = [];
        if (self.state.data) {
            self.state.data.forEach(function(record) {
                rows.push((
                    <TableRow hover role="checkbox" key={record.login}>
                        <TableCell><Checkbox color="primary" checked={record.isSelected} onChange={(e) => self.handleRowClick(e, record)} /></TableCell>
                        <TableCell>{record.code}</TableCell>
                        <TableCell><Checkbox color="primary" checked={record.is_identifier} disabled={true} /></TableCell>
                        <TableCell>{moment(record.update_datetime).format('LLLL')}</TableCell>
                        <TableCell>
                            <IconButton onClick={ () => self.props.history.push('/claims/' + record.code) }><Visibility /></IconButton>
                        </TableCell>
                    </TableRow>
                ));
            });
        }

        return (<div className="block">
            <Dialog open={self.state.isModalOpened} onClose={this.handleCloseModal}>
                <DialogTitle>{t('addClaim')}</DialogTitle>
                {self.state.isAddClaimLoading ? (<CircularProgress />) : (
                    <div>
                        <DialogContent>
                            <FormControl fullWidth={true}>
                                <form onSubmit={(e) => { e.preventDefault(); self.handleAddClaim(); }}>
                                    <InputLabel>{t('claimCode')}</InputLabel>
                                    <Input value={self.state.newClaimCode} name="newClaimCode" onChange={self.handleChangeFilter}  />
                                    <FormHelperText>{t('claimCodeShortDescription')}</FormHelperText>
                                </form>
                            </FormControl>
                        </DialogContent>
                        <DialogActions>
                            <Button  variant="raised" color="primary" onClick={self.handleAddClaim}>{t('addClaim')}</Button>
                        </DialogActions>
                    </div>
                )}
            </Dialog>
            <div className="block-header">
                <Grid container>
                    <Grid item md={5} sm={12}>
                        <h4>{t('claims')}</h4>
                        <i>{t('claimsShortDescription')}</i>
                    </Grid>
                    <Grid item md={7} sm={12}>
                        <ul className="breadcrumb float-md-right">
                            <li className="breadcrumb-item"><NavLink to="/">{t('websiteTitle')}</NavLink></li>
                            <li className="breadcrumb-item">{t('claims')}</li>
                        </ul>
                    </Grid>
                </Grid>

            </div>
            <div className="card">
                <div className="header">
                    <h4 style={{display: "inline-block"}}>{t('listOfClaims')}</h4>
                    <div style={{float: "right"}}>
                        {self.state.isRemoveDisplayed && (
                            <IconButton onClick={self.handleRemoveClaims}>
                                <Delete />
                            </IconButton>
                        )}
                        <IconButton onClick={this.handleClick}>
                            <MoreVert />
                        </IconButton>
                        <Menu anchorEl={self.state.anchorEl} open={Boolean(self.state.anchorEl)} onClose={self.handleClose}>
                            <MenuItem onClick={self.handleOpenModal}>{t('addClaim')}</MenuItem>
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
                                        <TableCell>{t('claimCode')}</TableCell>
                                        <TableCell>{t('claimUsedAsIdentifier')}</TableCell>
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
                                                <TextField value={this.state.selectedCode} name='selectedCode' onChange={this.handleChangeFilter} placeholder={t('Filter...')}/>
                                                <IconButton onClick={self.refreshData}><Search /></IconButton>
                                            </form>
                                        </TableCell>
                                        <TableCell></TableCell>
                                        <TableCell></TableCell>
                                        <TableCell></TableCell>
                                    </TableRow>
                                    {rows}
                                </TableBody>
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
            self.refreshData();
        });
        if (SessionStore.getSession().selectedOpenid) {
            self.refreshData();
        }
    }
}

export default translate('common', { wait: process && !process.release })(withRouter(Claims));