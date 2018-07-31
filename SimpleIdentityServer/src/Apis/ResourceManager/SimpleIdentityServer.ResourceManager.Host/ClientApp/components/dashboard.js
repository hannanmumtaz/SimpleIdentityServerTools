import React, { Component } from "react";
import { translate } from 'react-i18next';
import { NavLink } from 'react-router-dom';
import { Grid, Typography, CircularProgress, List, ListItem, ListItemText, IconButton } from 'material-ui';
import { ClientService, ScopeService, ResourceOwnerService, ClaimService, ResourceService } from '../services';
import { SessionStore } from '../stores';
import $ from 'jquery';
import Constants from '../constants';
import moment from 'moment';
import Visibility from '@material-ui/icons/Visibility'; 

class Dashboard extends Component {
    constructor(props) {
        super(props);
        this.refreshData = this.refreshData.bind(this);
        this.state = {
            isOpenidClientsLoading: false,
            isOpenidScopesLoading: false,
            isAuthClientsLoading: false,
            isAuthScopesLoading: false,
            isClaimsLoading: false,
            isUsersLoading: false,
            isLatestErrorsLoading: false,
            isLatestLogsLoading: false,
            isUmaResourcesLoading: false,
            nbOpenidClients: 0,
            nbAuthClients: 0,
            nbOpenidScopes: 0,
            nbAuthScopes: 0,
            nbClaims: 0,
            nbUsers: 0,
            nbUmaResources: 0,
            latestErrors: [],
            latestLogs: []
        };
    }

    refreshData() {
        var self = this;
        self.setState({
            isOpenidClientsLoading: true,
            isAuthClientsLoading: true,
            isOpenidScopesLoading: true,
            isAuthScopesLoading: true,
            isClaimsLoading: true,
            isUsersLoading: true,
            isLatestErrorsLoading: true,
            isLatestLogsLoading: true,
            isUmaResourcesLoading: true
        });
        // Openid clients
        ClientService.getAll('openid').then(function(result) {            
            self.setState({
                isOpenidClientsLoading: false,
                nbOpenidClients: result.length
            });
        }).catch(function() {
            self.setState({
                isOpenidClientsLoading: false,
                nbOpenidClients: 0
            });
        });
        // Oauth clients
        ClientService.getAll('auth').then(function(result) {            
            self.setState({
                isAuthClientsLoading: false,
                nbAuthClients: result.length
            });
        }).catch(function() {
            self.setState({
                isAuthClientsLoading: false,
                nbAuthClients: 0
            });
        });
        // Openid scopes
        ScopeService.getAll('openid').then(function(result) {           
            self.setState({
                isOpenidScopesLoading: false,
                nbOpenidScopes: result.length
            });
        }).catch(function() {           
            self.setState({
                isOpenidScopesLoading: false,
                nbOpenidScopes: 0
            });
        });
        // Auth scopes
        ScopeService.getAll('auth').then(function(result) {           
            self.setState({
                isAuthScopesLoading: false,
                nbAuthScopes: result.length
            });
        }).catch(function() {           
            self.setState({
                isAuthScopesLoading: false,
                nbAuthScopes: 0
            });
        });
        // Claims
        ClaimService.getAll().then(function(result) {           
            self.setState({
                isClaimsLoading: false,
                nbClaims: result.length
            });
        }).catch(function() {           
            self.setState({
                isClaimsLoading: false,
                nbClaims: 0
            });
        });
        // Users
        ResourceOwnerService.getAll().then(function(result) {
            self.setState({
                isUsersLoading: false,
                nbUsers: result.length
            });
        }).catch(function() {
            self.setState({
                isUsersLoading: false,
                nbUsers: 0
            });
        });
        // UMA resources
        ResourceService.getAll().then(function(result) {
            self.setState({
                isUmaResourcesLoading: false,
                nbUmaResources: result.length
            });
        }).catch(function() {
            self.setState({
                isUmaResourcesLoading: false,
                nbUmaResources: 0
            });
        });
        var eventSourceUrl  = Constants.eventSourceUrl;
        var getLatestErrorUrl = eventSourceUrl + "/events/.search?filter=where$(Verbosity eq '1') "+
            "orderby$on(CreatedOn),order(desc)&count=5";
        var getLatestLogsUrl = eventSourceUrl + "/events/.search?filter=where$(Verbosity eq '0') "+
            "orderby$on(CreatedOn),order(desc)&count=5";
        $.get(getLatestErrorUrl).done(function(result) {
            self.setState({
                latestErrors: result.content,
                isLatestErrorsLoading: false
            });
        }).fail(function() {
            self.setState({
                latestErrors: [],
                isLatestErrorsLoading: false
            });
        });
        $.get(getLatestLogsUrl).done(function(result) {
            self.setState({
                isLatestLogsLoading: false,
                latestLogs: result.content
            });
        }).fail(function() {
            self.setState({
                isLatestLogsLoading: false,
                latestLogs: []
            });
        });
    }

    render() {
        var self = this;
        const { t } = self.props;
        var lstError = [], lstLogs = [];
        if (self.state.latestErrors) {
            self.state.latestErrors.forEach(function(latestError) {
                lstError.push(
                    <ListItem dense button style={{overflow: 'hidden'}}>
                        <NavLink to={'/viewlog/' + latestError.Id}>
                            <IconButton>
                                <Visibility />
                            </IconButton>
                        </NavLink>
                        <ListItemText>
                            {moment(latestError.CreatedOn).format('LLLL')} : {JSON.parse(latestError.Payload).Message}
                        </ListItemText>
                    </ListItem>
                );
            });
            self.state.latestLogs.forEach(function(latestLog) {
                lstLogs.push(
                    <ListItem dense button style={{overflow: 'hidden'}}>
                        <NavLink to={'/viewlog/' + latestLog.Id}>
                            <IconButton>
                                <Visibility />
                            </IconButton>
                        </NavLink>
                        <ListItemText>
                            {moment(latestLog.CreatedOn).format('LLLL')} : {latestLog.Description}
                        </ListItemText>
                    </ListItem>
                );
            });
        }

        return (<div className="block">
            <div className="block-header">
                <Grid container>
                    <Grid item md={5} sm={12}>
                        <h4>{t('dashboardTitle')}</h4>
                        <i>{t('dashboardShortDescription')}</i>
                    </Grid>
                    <Grid item md={7} sm={12}>                        
                        <ul className="breadcrumb float-md-right">
                            <li className="breadcrumb-item"><NavLink to="/">{t('websiteTitle')}</NavLink></li>
                            <li className="breadcrumb-item">{t('dashboard')}</li>
                        </ul>
                    </Grid>
                </Grid>
            </div>
            <Grid container spacing={16}>
                {/* OPENID clients */}
                <Grid item sm={12} md={4}>
                    <div className="card">
                        {self.state.isOpenidClientsLoading ? (<div className="body"><CircularProgress /></div>) : (
                            <div className="body">
                                <Typography variant="display1">{self.state.nbOpenidClients}</Typography>
                                <Typography variant="caption" gutterBottom>{t('openidclients')}</Typography>
                            </div>
                        )}
                    </div>
                </Grid>
                {/* OAUTH2.0 clients */}
                <Grid item sm={12} md={4}>
                    <div className="card">
                        {self.state.isAuthClientsLoading ? (<div className="body"><CircularProgress /></div>) : (
                            <div className="body">
                                <Typography variant="display1">{self.state.nbAuthClients}</Typography>
                                <Typography variant="caption" gutterBottom>{t('oauthClients')}</Typography>
                            </div>
                        )}
                    </div>
                </Grid>
                {/* OPENID scopes */}
                <Grid item sm={12} md={4}>
                    <div className="card">
                        {self.state.isOpenidScopesLoading ? (<div className="body"><CircularProgress /></div>) : (
                            <div className="body">
                                <Typography variant="display1">{self.state.nbOpenidScopes}</Typography>
                                <Typography variant="caption" gutterBottom>{t('openidScopes')}</Typography>
                            </div>
                        )}
                    </div>
                </Grid>
                {/* AUTH scopes */}
                <Grid item sm={12} md={4}>
                    <div className="card">
                        {self.state.isAuthScopesLoading ? (<div className="body"><CircularProgress /></div>) : (
                            <div className="body">
                                <Typography variant="display1">{self.state.nbAuthScopes}</Typography>
                                <Typography variant="caption" gutterBottom>{t('authscopes')}</Typography>
                            </div>
                        )}
                    </div>
                </Grid>
                {/* Users */}
                <Grid item sm={12} md={4}>
                    <div className="card">
                        {self.state.isUsersLoading ? (<div className="body"><CircularProgress /></div>) : (
                            <div className="body">
                                <Typography variant="display1">{self.state.nbUsers}</Typography>
                                <Typography variant="caption" gutterBottom>{t('resourceOwners')}</Typography>
                            </div>
                        )}
                    </div>
                </Grid>
                {/* Claims */}
                <Grid item sm={12} md={4}>
                    <div className="card">
                        {self.state.isClaimsLoading ? (<div className="body"><CircularProgress /></div>) : (
                            <div className="body">
                                <Typography variant="display1">{self.state.nbClaims}</Typography>
                                <Typography variant="caption" gutterBottom>{t('claims')}</Typography>
                            </div>
                        )}
                    </div>
                </Grid>
                {/* UMA resources */}
                <Grid item sm={12} md={4}>
                    <div className="card">
                        {self.state.isClaimsLoading ? (<div className="body"><CircularProgress /></div>) : (
                            <div className="body">
                                <Typography variant="display1">{self.state.nbUmaResources}</Typography>
                                <Typography variant="caption" gutterBottom>{t('resources')}</Typography>
                            </div>
                        )}
                    </div>
                </Grid>
                {/* Latest logs */}
                <Grid item sm={12} md={6}>
                    <div className="card">
                        <div className="header">
                            <h4 style={{display: "inline-block"}}>{t('latestLogs')}</h4>
                        </div>
                        {self.state.isLatestLogsLoading ? (<div className="body"><CircularProgress /></div>) : (
                            <div className="body">
                                <List>
                                    {lstLogs}
                                </List>
                            </div>
                        )}
                    </div>
                </Grid>
                {/* Latest errors */}
                <Grid item sm={12} md={6}>
                    <div className="card">
                        <div className="header">
                            <h4 style={{display: "inline-block"}}>{t('latestErrors')}</h4>
                        </div>
                        {self.state.isLatestErrorsLoading ? (<div className="body"><CircularProgress /></div>) : (
                            <div className="body">
                                <List>
                                    {lstError}
                                </List>
                            </div>
                        )}
                    </div>
                </Grid>
            </Grid>
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

export default translate('common', { wait: process && !process.release })(Dashboard);