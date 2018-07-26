import React, { Component } from "react";
import { translate } from 'react-i18next';
import { NavLink } from "react-router-dom";
import { ResourceOwnerService, ClaimService } from '../services';
import AppDispatcher from '../appDispatcher';
import Constants from '../constants';
import Save from '@material-ui/icons/Save';
import { SessionStore } from '../stores';
import { ChipsSelector } from './common';
import $ from 'jquery';

import { CircularProgress, IconButton, Select, MenuItem, Checkbox, Typography, Grid, Button, Paper, Chip, List, ListItem, ListItemText } from 'material-ui';
import Table, { TableBody, TableCell, TableHead, TableRow, TableFooter } from 'material-ui/Table';
import { FormControl, FormHelperText } from 'material-ui/Form';
import Input, { InputLabel } from 'material-ui/Input';
import { withStyles } from 'material-ui/styles';
import Delete from '@material-ui/icons/Delete';
import Visibility from '@material-ui/icons/Visibility';

const styles = theme => ({
  margin: {
    margin: theme.spacing.unit,
  }
});

class ViewResourceOwner extends Component {
    constructor(props) {
        super(props);
        this.refreshData = this.refreshData.bind(this);
        this.saveClaims = this.saveClaims.bind(this);
        this.savePassword = this.savePassword.bind(this);
        this.handleChangeProperty = this.handleChangeProperty.bind(this);
        this.handleChangeClaimValue = this.handleChangeClaimValue.bind(this);
        this.handleAddClaim = this.handleAddClaim.bind(this);
        this.handleRemoveClaim = this.handleRemoveClaim.bind(this);
        this.state = {
        	isLoading: true,
            roLogin: '',
            roClaims: [],
            newpassword: '',
            passwordConfirmation: '',
            claimType: '',
            claimValue: '',
            supportedClaims: [],
            isRemoveDisplayed: false
        };

	}

	/**
	* Display the user informations.
	*/
	refreshData() {
		var self = this;
		const { t } = self.props;
		self.setState({
			isLoading: true
		});
        var profile = SessionStore.getSession();
        Promise.all([
            ResourceOwnerService.get(self.state.login),
            $.get(profile.selectedOpenid.url)
        ]).then(function(values) {
            var user = values[0];
            var wellKnownConfiguration = values[1];
            var claimType = wellKnownConfiguration['claims_supported'][0];
            self.setState({
                isLoading: false,
                roClaims: user.claims,
                roLogin: user.login,
                claimType: claimType,
                supportedClaims: wellKnownConfiguration['claims_supported']
            });
        }).catch(function(e) {
            self.setState({
                isLoading: false
            });
            AppDispatcher.dispatch({
                actionName: Constants.events.DISPLAY_MESSAGE,
                data: t('resourceOwnerCannotBeRetrieved')
            });     
        });
	}

	/**
	* Save the user.
	*/
	saveClaims() {
        var self = this;
        const {t} = self.props;
        var roClaims = self.state.roClaims;
        var request = {
            login: self.state.login,
            claims: roClaims
        };
        self.setState({
            isLoading: true
        });
        ResourceOwnerService.updateClaims(request).then(function() {
            self.setState({
                isLoading: false
            });
            AppDispatcher.dispatch({
                actionName: Constants.events.DISPLAY_MESSAGE,
                data: t('resourceOwnerClaimsUpdated')
            }); 
        }).catch(function() {
            self.setState({
                isLoading: false
            });
            AppDispatcher.dispatch({
                actionName: Constants.events.DISPLAY_MESSAGE,
                data: t('resourceOwnerClaimsCannotBeUpdated')
            }); 
        });
	}

    /**
    * Save the password
    */
    savePassword() {
        var self = this;
        const {t} = self.props;
        if (self.state.newpassword !== self.state.passwordConfirmation) {
            AppDispatcher.dispatch({
                actionName: Constants.events.DISPLAY_MESSAGE,
                data: t('thePasswordDoesntMatch')
            });
            return;
        }

        self.setState({
            isLoading: true
        });
        var request = {
            login: self.state.login,
            password: self.state.newpassword
        };
        ResourceOwnerService.updatePassword(request).then(function() {
            self.setState({
                isLoading: false
            });
            AppDispatcher.dispatch({
                actionName: Constants.events.DISPLAY_MESSAGE,
                data: t('resourceOwnerPasswordUpdated')
            }); 
        }).catch(function() {
            self.setState({
                isLoading: false
            });
            AppDispatcher.dispatch({
                actionName: Constants.events.DISPLAY_MESSAGE,
                data: t('resourceOwnerPasswordCannotBeUpdated')
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
    * Handle claim value.
    */
    handleChangeClaimValue(e, record) {
        record.value = e.target.value;
        var claims = this.state.claims;
        this.setState({
            claims: claims
        });
    }

    /**
    * Add a claim to the list.
    */
    handleAddClaim() {
        var self = this;
        var claimType = self.state.claimType;
        var claimValue = self.state.claimValue;   
        var roClaims = self.state.roClaims;     
        var roClaimsTypes = roClaims.map(function(c) { return c.key; });
        if (!claimType || claimType === '' || !claimValue || claimValue === '' || roClaimsTypes.indexOf(claimType) !== -1) {
            return;
        }

        var record = { key : claimType, value : claimValue };
        roClaims.push(record);
        self.setState({
            claimValue: '',
            roClaims: roClaims
        });
    }

    /**
    * Remove the selected claim.
    */
    handleRemoveClaim(claim) {
        const roClaims = this.state.roClaims;
        const claimIndex = roClaims.indexOf(claim);
        roClaims.splice(claimIndex, 1);        
        this.setState({
            roClaims: roClaims
        });
    }

    render() {
    	var self = this;
    	const { t, classes } = self.props;
        var claims = [], userClaims = [];
        if (self.state.supportedClaims) {
            self.state.supportedClaims.forEach(function(claim) {
                claims.push((<MenuItem key={claim} value={claim}>{t(claim)}</MenuItem>));
            });
        }

        if (self.state.roClaims) {
            self.state.roClaims.forEach(function(claim) {
                userClaims.push(
                    <ListItem dense button style={{overflow: 'hidden'}}>
                        <IconButton onClick={() => self.handleRemoveClaim(claim)}>
                            <Delete />
                        </IconButton>
                        <NavLink to={"/claims/" + claim.key}>
                            <IconButton>
                                <Visibility />
                            </IconButton>
                        </NavLink>
                        <ListItemText>{claim.key} : {claim.value}</ListItemText>
                    </ListItem>
                );
            });
        }

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
                            <li className="breadcrumb-item"><NavLink to="/resourceowners">{t('resourceOwners')}</NavLink></li>
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
                    	<Grid container spacing={40}>
                    		<Grid item md={5} sm={12}>
		                        {/* Login */}
		                        <FormControl fullWidth={true} className={classes.margin} disabled={true}>
		                            <InputLabel>{t('roLogin')}</InputLabel>
		                            <Input value={self.state.roLogin} disabled={true} />
		                            <FormHelperText>{t('roLoginDescription')}</FormHelperText>
		                        </FormControl> 
                                <form onSubmit={(e) => { e.preventDefault(); self.savePassword(); }}>
                                    {/* New password */ }
                                    <FormControl fullWidth={true} className={classes.margin}>
                                        <InputLabel>{t('newPassword')}</InputLabel>
                                        <Input type='password' value={self.state.newpassword} onChange={self.handleChangeProperty} name='newpassword' />
                                        <FormHelperText>{t('newPasswordDescription')}</FormHelperText>
                                    </FormControl> 
                                    {/* Confirm password */ }
                                    <FormControl fullWidth={true} className={classes.margin}>
                                        <InputLabel>{t('passwordConfirmation')}</InputLabel>
                                        <Input type='password' value={self.state.passwordConfirmation} onChange={self.handleChangeProperty} name='passwordConfirmation'/>
                                        <FormHelperText>{t('passwordConfirmationDescription')}</FormHelperText>
                                    </FormControl> 
                                    <Button variant="raised" color="primary" onClick={self.savePassword}>{t('savePassword')}</Button>
                                </form>                			
                    		</Grid>
                    		<Grid item md={5} sm={12}>
                                {/* Claims */}
                                <FormControl fullWidth={true}>
                                    <form onSubmit={(e) => { e.preventDefault(); self.handleAddClaim(); }}>
                                        <Select value={self.state.claimType} onChange={self.handleChangeProperty} name="claimType">
                                            {claims}
                                        </Select>                                    
                                        <FormControl className={classes.margin}>
                                            <InputLabel>{t('claimValue')}</InputLabel>
                                            <Input value={self.state.claimValue} name="claimValue" onChange={self.handleChangeProperty}  />
                                        </FormControl>
                                        <Button variant="raised" color="primary" onClick={this.handleAddClaim}>{t('addClaim')}</Button>
                                        <Button variant="raised" color="primary" onClick={this.saveClaims}>{t('saveClaims')}</Button>
                                    </form>
                                </FormControl>
                                <List>
                                    {userClaims}
                                </List>
                    		</Grid>
                    	</Grid>
                    )}
                </div>
            </div>
    	</div>);
    }

    componentDidMount() {
        var self = this;
        SessionStore.addChangeListener(function() {
            self.setState({
                login: self.props.match.params.id
            }, function() {
                self.refreshData();
            });
        });
        
        if (SessionStore.getSession().selectedOpenid) {
            self.setState({
                login: self.props.match.params.id
            }, function() {
                self.refreshData();
            });
        }
    }
}

export default translate('common', { wait: process && !process.release })(withStyles(styles)(ViewResourceOwner));