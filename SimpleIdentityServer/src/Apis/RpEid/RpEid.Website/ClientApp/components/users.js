import React, { Component } from "react";
import { translate } from 'react-i18next';
import Table, { TableBody, TableCell, TableHead, TableRow, TableFooter, TablePagination, TableSortLabel } from 'material-ui/Table';
import { Grid, IconButton, CircularProgress, Checkbox } from 'material-ui';
import Done from '@material-ui/icons/Done';
import Send from '@material-ui/icons/Send';
import { AccountService } from '../services';
import AppDispatcher from '../appDispatcher';
import Constants from '../constants';

class Users extends Component {
    constructor(props) {
        super(props);
        this.enableAccounts = this.enableAccounts.bind(this);
        this.refresh = this.refresh.bind(this);
        this.handleRowClick = this.handleRowClick.bind(this);
        this.handleAllSelections = this.handleAllSelections.bind(this);
        this.handleChangePage = this.handleChangePage.bind(this);
        this.handleChangeRowsPage = this.handleChangeRowsPage.bind(this);
        this.state = {
            users: [],
            isResendDisplayed: false,
            isLoading: false,
            page: 0,
            pageSize: 5,
            count: 0
        };
    }

    enableAccounts() {
        var self = this;
        const { t } = self.props;
        var users = self.state.users.filter(function (user) { return user.isChecked && !user.isGranted; });
        if (users.length === 0) {
            return;
        }

        self.setState({
            isLoading: true
        });
        var ops = [];
        users.forEach(function (user) {
            ops.push(AccountService.grant(user.subject));
        });
        Promise.all(ops).then(function () {
            self.setState({
                isLoading: false
            });
            AppDispatcher.dispatch({
                actionName: Constants.events.DISPLAY_MESSAGE,
                data: t('accessHaveBeenGranted')
            });
            self.refresh();
        }).catch(function (e) {
            console.log(e);
            AppDispatcher.dispatch({
                actionName: Constants.events.DISPLAY_MESSAGE,
                data: t('accessCannotBeGranted')
            });
            self.setState({
                isLoading: false
            });
        });
    }

    refresh() {
        var self = this;
        const { t } = self.props;
        self.setState({
            isLoading: true
        });
        var startIndex = self.state.page * self.state.pageSize;
        var request = { start_index: startIndex, count: self.state.pageSize, order: 1, is_confirmed: false };
        AccountService.search(request).then(function (result) {
            var users = [];
            if (result.content) {
                result.content.forEach(function (c) {
                    users.push({
                        subject: c.subject,
                        email: c.email,
                        name: c.name,
                        isGranted: c.isGranted,
                        isChecked: false
                    });
                });
            }

            console.log(result);
            self.setState({
                isLoading: false,
                users: users
            });            
        }).catch(function () {
            AppDispatcher.dispatch({
                actionName: Constants.events.DISPLAY_MESSAGE,
                data: t('accountsCannotBeDisplayed')
            });
            self.setState({
                isLoading: false
            });
        });
    }

    handleRowClick(e, user) {
        user.isChecked = e.target.checked;
        var users = this.state.users;
        var nbSelectedRecords = users.filter(function (r) { return r.isChecked && !r.isGranted; }).length;
        this.setState({
            users: users,
            isGrantedDispayed: nbSelectedRecords > 0
        });
    }

    handleAllSelections(e) {
        var checked = e.target.checked;
        var users = this.state.users;
        users.forEach(function (r) { r.isChecked = checked; });
        var nbSelectedRecords = users.filter(function (r) { return r.isChecked && !r.isGranted; }).length;
        this.setState({
            users: users,
            isGrantedDispayed: nbSelectedRecords > 0
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
            self.refresh();
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
            self.refresh();
        });
    }

    render() {
        const { t } = this.props;
        var self = this;
        var rows = [];
        if (self.state.users) {
            self.state.users.forEach(function (user) {
                rows.push(
                    <TableRow key={user.subject}>
                        <TableCell><Checkbox color="primary" checked={user.isChecked} onChange={(e) => self.handleRowClick(e, user)} /></TableCell>
                        <TableCell>{user.subject}</TableCell>
                        <TableCell>{user.name}</TableCell>
                        <TableCell>{user.email}</TableCell>
                        <TableCell><Checkbox color="primary" checked={user.isGranted} disabled={true} /></TableCell>
                    </TableRow>
                );
            });
        }

        return (<div className="block">
            <div className="block-header">
                <Grid container>
                    <Grid item md={5} sm={12}>
                        <h4>{t('usersTitle')}</h4>
                        <i>{t('usersDescription')}</i>
                     </Grid>
                     <Grid item md={7} sm={12}>               
                        <ul className="breadcrumb float-md-right">
                            <li className="breadcrumb-item">{t('websiteTitle')}</li>
                            <li className="breadcrumb-item">{t('users')}</li>
                        </ul>
                     </Grid>
                </Grid>
            </div>
            <div className="card">
                <div className="header">
                    <h4 style={{ display: "inline-block" }}>{t('listOfUsers')}</h4>
                    <div style={{ float: "right" }}>
                        {self.state.isGrantedDispayed && (
                            <IconButton onClick={self.enableAccounts}>
                                <Done />
                            </IconButton>
                        )}
                    </div>
                </div>
                <div className="body">
                    {self.state.isLoading ? (<CircularProgress />) : (
                        <div>
                            <Table>
                                <TableHead>
                                    <TableRow>
                                        <TableCell></TableCell>
                                        <TableCell>{t('subject')}</TableCell>
                                        <TableCell>{t('name')}</TableCell>
                                        <TableCell>{t('email')}</TableCell>
                                        <TableCell>{t('isGranted')}</TableCell>
                                    </TableRow>
                                </TableHead>
                                <TableBody>
                                    <TableRow>
                                        <TableCell><Checkbox color="primary" onChange={self.handleAllSelections} /></TableCell>
                                        <TableCell></TableCell>
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
        this.refresh();
    }
}

export default translate('common', { wait: process && !process.release })(Users);