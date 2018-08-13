import React, { Component } from "react";
import { withRouter } from 'react-router-dom';
import { translate } from 'react-i18next';
import { UserProfileService, AuthProviderService } from '../../services';
import AppDispatcher from '../../appDispatcher';
import Constants from '../../constants';
import { List, ListItem, ListItemText, Select, MenuItem, CircularProgress, Button, IconButton } from 'material-ui';
import { FormControl, FormHelperText } from 'material-ui/Form';
import Input, { InputLabel } from 'material-ui/Input';
import $ from 'jquery';
import Delete from '@material-ui/icons/Delete';

class UserProfileTab extends Component {
    constructor(props) {
        super(props);
        this.refreshData = this.refreshData.bind(this);
        this.handleChangeProperty = this.handleChangeProperty.bind(this);
        this.linkProfile = this.linkProfile.bind(this);
        this.unlinkProfile = this.unlinkProfile.bind(this);
        this.state = {
        	isLoading: true,
        	login: null,
        	externalSubject: null,
        	selectedAuthProvider: null,
        	authProviders: [],
        	profiles : []
        };
    }

    /**
    * Refresh the data.
    */
    refreshData(login) {
    	var self = this;
    	const { t } = self.props;
    	self.setState({
    		isLoading: true,
    		login: login
    	});
    	Promise.all([UserProfileService.getUserProfiles(login), AuthProviderService.getAuthProviders()]).then(function(values) {
    		var userProfiles = values[0];
    		var authProviders = values[1];
    		self.setState({
    			authProviders: authProviders,
    			profiles: userProfiles,
    			isLoading: false,
    			selectedAuthProvider: authProviders[0]['authentication_scheme']
    		});
    	}).catch(function(e) {
            AppDispatcher.dispatch({
                actionName: Constants.events.DISPLAY_MESSAGE,
                data: t('resourceOwnerProfilesCannotBeRetrieved')
            }); 
    		self.setState({
    			isLoading: false,
    			profiles: [],
    			authProviders: []
    		});
    	});
    }
    
    /**
    * Change the property.
    */
    handleChangeProperty(e) {
        this.setState({
            [e.target.name]: e.target.value
        });
    }

    /**
    * Link the profile.
    */
    linkProfile() {
    	var self = this;
    	const { t } = self.props;
    	var request = {
    		user_id: self.state.externalSubject,
    		issuer: self.state.selectedAuthProvider
    	};
    	if (!self.state.externalSubject || !self.state.selectedAuthProvider || self.state.externalSubject === '') {
    		return;
    	}

    	var filteredProfile = self.state.profiles.filter(function(p) {
    		return p['issuer'] === self.state.selectedAuthProvider;
    	});
    	if (filteredProfile.length > 0) {    		
            AppDispatcher.dispatch({
                actionName: Constants.events.DISPLAY_MESSAGE,
                data: t('authProviderAlreadyLinked')
            }); 
    		return;
    	}

    	self.setState({
    		isLoading: true
    	});
    	UserProfileService.linkUserProfile(self.state.login, request).then(function() {
            AppDispatcher.dispatch({
                actionName: Constants.events.DISPLAY_MESSAGE,
                data: t('resourceOwnerProfileLinked')
            }); 
            var profiles = self.state.profiles;
            profiles.push({
            	authentication_scheme: self.state.selectedAuthProvider,
            	display_name: self.state.externalSubject
            });
    		self.setState({
    			isLoading: false,
    			profiles: profiles
    		});
    	}).catch(function() {
            AppDispatcher.dispatch({
                actionName: Constants.events.DISPLAY_MESSAGE,
                data: t('resourceOwnerProfileCannotBeLinked')
            }); 
    		self.setState({
    			isLoading: false
    		});
    	});
    }

    /**
    * Unlink the profile.
    */
    unlinkProfile(profile) {
    	var self = this;
    	const { t } = self.props;
    	self.setState({
    		isLoading: true
    	});
    	UserProfileService.unlinkUserProfile(self.state.login, profile.user_id).then(function() {
            AppDispatcher.dispatch({
                actionName: Constants.events.DISPLAY_MESSAGE,
                data: t('resourceOwnerProfileUnlinked')
            }); 
            var profiles = self.state.profiles;
            var index = profiles.indexOf(profile);
            profiles.splice(index, 1);
    		self.setState({
    			isLoading: false,
    			profiles: profiles
    		});    		
    	}).catch(function(e) {
            AppDispatcher.dispatch({
                actionName: Constants.events.DISPLAY_MESSAGE,
                data: t('resourceOwnerProfileCannotBeUnlinked')
            }); 
    		self.setState({
    			isLoading: false
    		});    		
    	});
    }

    render() {
    	var self = this;
    	const {t} = self.props;
        var style = {};
        var authProviders = [];
        var profiles = [];
        if (self.props.hidden) {
            style = { display : "none" };
        }

        if (self.state.authProviders) {
        	self.state.authProviders.forEach(function(authProvider) {
        		authProviders.push(
        			<MenuItem key={authProvider['authentication_scheme']} value={authProvider['authentication_scheme']}>
        				{authProvider['display_name']}
        			</MenuItem>
        		);
        	});
        }

        if (self.state.profiles) {
        	self.state.profiles.forEach(function(profile) {
        		profiles.push(
        			<ListItem dense button style={{overflow: 'hidden'}}>
        				<IconButton  onClick={() => self.unlinkProfile(profile)}>
        					<Delete />
        				</IconButton>
        				<ListItemText>{profile.issuer}</ListItemText>
        			</ListItem>
        		);
        	});
        }

    	return (<div style={style}>
    		{ self.state.isLoading ? (<CircularProgress />) : (
    			<div>
		    		<form onSubmit={(e) => { e.preventDefault(); self.linkProfile(); }}>
		    			<Select style={{margin: "5px"}} value={self.state.selectedAuthProvider} onChange={self.handleChangeProperty} name="selectedAuthProvider">
		    				{authProviders}
		    			</Select>
		                <FormControl style={{margin: "5px"}}>
		                	<InputLabel>{t('externalSubject')}</InputLabel>
		                    <Input type="text" value={self.state.externalSubject} name="externalSubject" onChange={self.handleChangeProperty} />
		                    <FormHelperText>{t('externalSubjectDescription')}</FormHelperText>
		                </FormControl>
		                <Button style={{margin: "5px"}} variant="raised" color="primary" onClick={self.linkProfile}>{t('linkProfile')}</Button>
		    		</form>
		    		<List>
		    			{profiles}
		    		</List>
	    		</div>
    		)}
    	</div>);
    }
}


export default translate('common', { wait: process && !process.release, withRef : true })(UserProfileTab);