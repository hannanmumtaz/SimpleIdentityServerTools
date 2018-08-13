import React, { Component } from "react";
import { translate } from 'react-i18next';
import { NavLink, Link } from "react-router-dom";
import { UserInfoTab, UserProfileTab } from './userTabs';
import AppDispatcher from '../appDispatcher';
import { SessionStore } from '../stores';
import Constants from '../constants';
import Save from '@material-ui/icons/Save';
import { ChipsSelector } from './common';
import $ from 'jquery';

import { CircularProgress, IconButton, Select, MenuItem, Checkbox, Typography, Grid, Button, Paper, Chip, List, ListItem, ListItemText } from 'material-ui';
import Tabs, { Tab } from 'material-ui/Tabs';
import Input, { InputLabel } from 'material-ui/Input';


class ViewResourceOwner extends Component {
    constructor(props) {
        super(props);
        this.handleTabChange = this.handleTabChange.bind(this);
        this.state = {
            tabIndex: 0,
            login: '',
            isLoading: true
        };
	}

    /**
    * Refresh the data.
    */
    refreshData() {      
        this.userInfoTab.getWrappedInstance().refreshData(this.state.login);
        this.userProfileTab.getWrappedInstance().refreshData(this.state.login);
    }

    /**
    * Change tab.
    */
    handleTabChange(evt, val) {
        this.setState({
            tabIndex: val
        });
    }

    /**
    * Display the view.
    */
    render() {
        var self = this;
        const { t } = self.props;
    	return (<div className="block">
            <div className="block-header">
                <Grid container>
                    <Grid item md={5} sm={12}>
                        <h4>{t('resourceOwner')}</h4>
                        <i>{t('resourceOwnerDescription')}</i>
                    </Grid>
                    <Grid item md={7} sm={12}>                        
                        <ul className="breadcrumb float-md-right">
                            <li className="breadcrumb-item"><NavLink to="/">{t('websiteTitle')}</NavLink></li>
                            <li className="breadcrumb-item"><NavLink to="/users">{t('resourceOwners')}</NavLink></li>
                            <li className="breadcrumb-item">{t('resourceOwner')}</li>
                        </ul>
                    </Grid>
                </Grid>
            </div>
            <div className="card">
                <div className="header">
                	 <h4 style={{display: "inline-block"}}>{t('resourceOwnerInformation')}</h4>
                </div>
                <div className="body">
                    { self.state.isLoading ? (<CircularProgress />) : (
                        <div>
                            <Tabs indicatorColor="primary" value={self.state.tabIndex} onChange={self.handleTabChange}>
                                <Tab label={t('resourceOwnerSettings')} component={Link}  to={"/users/" + self.state.login} />
                                <Tab label={t('resourceOwnerProfiles')} component={Link}  to={"/users/" + self.state.login + "/profiles"} />
                            </Tabs>
                            <UserInfoTab ref={ ref => this.userInfoTab = ref } hidden={self.state.tabIndex !== 0} />
                            <UserProfileTab ref={ ref => this.userProfileTab = ref } hidden={self.state.tabIndex !== 1} />
                        </div>
                    )}
                </div>
            </div>
    	</div>);
    }

    componentDidMount() {
        var self = this;
        var tabIndex = 0;
        if (self.props.match.params.action === 'profiles') {
            tabIndex = 1;
        }

        SessionStore.addChangeListener(function() {
            self.setState({
                login: self.props.match.params.id,
                tabIndex: tabIndex,
                isLoading: false
            }, function() {
                self.refreshData();
            });
        });
        
        if (SessionStore.getSession().selectedOpenid) {
            self.setState({
                login: self.props.match.params.id,
                tabIndex: tabIndex,
                isLoading: false
            }, function() {
                self.refreshData();
            });
        }
    }
}

export default translate('common', { wait: process && !process.release })(ViewResourceOwner);