import React, { Component } from "react";
import AppDispatcher from '../appDispatcher';
import Constants from '../constants';
import { ScimService } from '../services';
import { withRouter } from 'react-router-dom';
import { NavLink } from 'react-router-dom';
import { translate } from 'react-i18next';
import { TextField , Button, Grid, IconButton, CircularProgress } from 'material-ui';
import Table, { TableBody, TableCell, TableHead, TableRow, TableFooter, TablePagination, TableSortLabel } from 'material-ui/Table';
import Visibility from '@material-ui/icons/Visibility'; 

class ScimResources extends Component {
    constructor(props) {
        super(props);
        this.refreshUsers = this.refreshUsers.bind(this);
        this.refreshGroups = this.refreshGroups.bind(this);
        this.handleChangePageUser = this.handleChangePageUser.bind(this);
        this.handleChangeRowsPageUser = this.handleChangeRowsPageUser.bind(this);
        this.handleChangePageGroup = this.handleChangePageGroup.bind(this);
        this.handleChangeRowsPageGroup = this.handleChangeRowsPageGroup.bind(this);
        this.state = {
            isUsersLoading: false,
            pageUser: 0,
            pageSizeUser: 5,
            countUser: 0,
            users: [],
            isGroupsLoading: false,
            pageGroup: 0,
            pageSizeGroup: 5,
            countGroup: 0,
            groups: []
        };
    }

    /**
    * Display the users.
    */
    refreshUsers() {
        var self = this;
        var startIndexUser = self.state.pageUser * self.state.pageSizeUser;
        self.setState({
            isUsersLoading: true
        });
        var userRequest = { startIndex: startIndexUser, count: self.state.pageSizeUser};
        ScimService.searchUsers(userRequest).then(function(result) {
            var users = [];
            if (result.content && result.content.Resources) {
                users = result.content.Resources;
            }

            self.setState({
                isUsersLoading: false,
                users: users
            });
        }).catch(function() {
            self.setState({
                isUsersLoading: false,
                users: []
            });
            AppDispatcher.dispatch({
                actionName: Constants.events.DISPLAY_MESSAGE,
                data: t('userResourcesCannotBeRetrieved')
            });
        });
    }

    /**
    * Display the groups.
    */
    refreshGroups() {
        var self = this;
        var startIndexGroup = self.state.pageGroup * self.state.pageSizeGroup;
        self.setState({
            isGroupsLoading: true
        });
        var groupRequest = { startIndex: startIndexGroup, count: self.state.pageSizeGroup };
        ScimService.searchGroups(groupRequest).then(function(result) {
            var groups = [];
            if (result.content && result.content.Resources) {
                groups = result.content.Resources;
            }

            self.setState({
                isGroupsLoading: false,
                groups: groups
            });
        }).catch(function() {
            self.setState({
                isGroupsLoading: false,
                groups: []
            });
            AppDispatcher.dispatch({
                actionName: Constants.events.DISPLAY_MESSAGE,
                data: t('groupResourcesCannotBeRetrieved')
            });
        });
    }

    handleChangePageUser(evt, page) {
        var self = this;
        this.setState({
            pageUser: page
        }, () => {
            self.refreshUsers();
        });
    }

    handleChangeRowsPageUser(evt) {
        var self = this;
        self.setState({
            pageSizeUser: evt.target.value
        }, () => {
            self.refreshUsers();
        });
    }

    handleChangePageGroup(evt, page) {
        var self = this;
        this.setState({
            pageGroup: page
        }, () => {
            self.refreshGroups();
        });
    }

    handleChangeRowsPageGroup() {
        var self = this;
        self.setState({
            pageSizeGroup: evt.target.value
        }, () => {
            self.refreshGroups();
        });
    }

    render() {
        var self = this;
        const { t } = self.props;
        var userRows = [];
        var groupRows = [];
        if (self.state.users) {
            self.state.users.forEach(function(user) {
                userRows.push(
                    <TableRow>
                        <TableCell>{user.id}</TableCell>
                        <TableCell>{JSON.stringify(user.meta)}</TableCell>
                        <TableCell>
                            <IconButton onClick={ () => self.props.history.push('/scimResources/user/' + user.id) }><Visibility /></IconButton>
                        </TableCell>
                    </TableRow>
                );
            });
        }

        if (self.state.groups) {
            self.state.groups.forEach(function(group) {
                groupRows.push(
                    <TableRow>
                        <TableCell>{group.id}</TableCell>
                        <TableCell>{JSON.stringify(user.meta)}</TableCell>
                        <TableCell>
                            <IconButton onClick={ () => self.props.history.push('/scimResources/group/' + group.id) }><Visibility /></IconButton>
                        </TableCell>
                    </TableRow>
                );
            });
        }

        return (<div className="block">
            <div className="block-header">
                <Grid container>
                    <Grid item md={5} sm={12}>
                        <h4>{t('scimResources')}</h4>
                        <i>{t('scimResourcesDescription')}</i>
                    </Grid>
                    <Grid item md={7} sm={12}>                        
                        <ul className="breadcrumb float-md-right">
                            <li className="breadcrumb-item"><NavLink to="/">{t('websiteTitle')}</NavLink></li>
                            <li className="breadcrumb-item">{t('scimResources')}</li>
                        </ul>
                    </Grid>
                </Grid>
            </div>
            <Grid container spacing={40}>
                <Grid item md={12}>
                    <div className="card">
                        { self.state.isUsersLoading ? ( <CircularProgress /> ) : (
                            <div>
                                <div className="header">
                                    <h4 style={{display: "inline-block"}}>{t('scimUsers')}</h4>
                                </div>
                                <div className="body">
                                    <Table>
                                        <TableHead>
                                            <TableRow>
                                                <TableCell>{t('scimId')}</TableCell>
                                                <TableCell>{t('scimMetadata')}</TableCell>
                                                <TableCell></TableCell>
                                            </TableRow>
                                        </TableHead>
                                        <TableBody>
                                            {userRows}
                                        </TableBody>
                                    </Table>
                                    <TablePagination component="div" count={self.state.countUser} rowsPerPage={self.state.pageSizeUser} page={this.state.pageUser} onChangePage={self.handleChangePageUser} onChangeRowsPerPage={self.handleChangeRowsPageUser} />
                                </div>
                            </div>
                        )}
                    </div>
                </Grid>
                <Grid item md={12}>
                    <div className="card">
                        { self.state.isGroupsLoading ? ( <CircularProgress /> ) : (
                            <div>
                                <div className="header">
                                    <h4 style={{display: "inline-block"}}>{t('scimGroups')}</h4>
                                </div>
                                <div className="body">
                                    <Table>
                                        <TableHead>
                                            <TableRow>
                                                <TableCell>{t('scimId')}</TableCell>
                                                <TableCell>{t('scimMetadata')}</TableCell>
                                                <TableCell></TableCell>                 
                                            </TableRow>           
                                        </TableHead>
                                        <TableBody>
                                            {groupRows}
                                        </TableBody>
                                        <TablePagination component="div" count={self.state.countGroup} rowsPerPage={self.state.pageSizeGroup} page={this.state.pageGroup} onChangePage={self.handleChangePageGroup} onChangeRowsPerPage={self.handleChangeRowsPageGroup} />
                                    </Table>
                                </div>
                            </div>
                        )}
                    </div>
                </Grid>
            </Grid>
        </div>);
    }

    componentDidMount() {
        var self = this;
        self.refreshUsers();
        self.refreshGroups();
    }
}

export default translate('common', { wait: process && !process.release })(withRouter(ScimResources));