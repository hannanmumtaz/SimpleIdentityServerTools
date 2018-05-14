import React, { Component } from "react";
import { translate } from 'react-i18next';
import { NavLink } from "react-router-dom";
import { ResourceOwnerService, ClaimService } from '../services';
import AppDispatcher from '../appDispatcher';
import Constants from '../constants';
import Save from '@material-ui/icons/Save';

import { CircularProgress, IconButton, Select, MenuItem, Checkbox, Typography, Grid } from 'material-ui';
import Table, { TableBody, TableCell, TableHead, TableRow, TableFooter } from 'material-ui/Table';
import { FormControl, FormHelperText } from 'material-ui/Form';
import Input, { InputLabel } from 'material-ui/Input';
import { withStyles } from 'material-ui/styles';

const styles = theme => ({
  margin: {
    margin: theme.spacing.unit,
  }
});

class ViewUser extends Component {
    constructor(props) {
        super(props);
        this.refreshData = this.refreshData.bind(this);
        this.saveUser = this.saveUser.bind(this);
        this.handleChangeProperty = this.handleChangeProperty.bind(this);
        this.handleRowClick = this.handleRowClick.bind(this);
        this.handleChangeClaimValue = this.handleChangeClaimValue.bind(this);
        this.state = {
        	isLoading: true,
        	login: '',
        	user: {},
            claims: []
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
        var request = { start_index: 0, count: 500 };
        Promise.all([
            ResourceOwnerService.get(self.state.login),
            ClaimService.search(request)
        ]).then(function(values) {
            var user = values[0];
            var claims = [];
            values[1].content.forEach(function(claim) {
                var record = {
                    key: claim.key,
                    value: '',
                    isChecked: false,
                    is_identifier: claim.is_identifier
                };
                if (user.claims) {
                    var userClaim = user.claims.filter(function(c) { return c.key === claim.key; });
                    if (userClaim.length !== 0) {
                        record.value = userClaim[0].value;
                        record.isChecked = true;
                    }
                    claim.isChecked = true;
                }

                claims.push(record);
            });
            self.setState({
                isLoading: false,
                user: user,
                claims: claims
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
	saveUser() {
        var self = this;
        const {t} = self.props;
        var claims = self.state.claims.filter(function(claim) { return claim.isChecked; });
        var user = self.state.user;
        user.claims = claims;
        self.setState({
            isLoading: true
        });
        ResourceOwnerService.update(user).then(function() {
            self.setState({
                isLoading: false
            });
            AppDispatcher.dispatch({
                actionName: Constants.events.DISPLAY_MESSAGE,
                data: t('resourceOwnerUpdated')
            }); 
        }).catch(function() {
            self.setState({
                isLoading: false
            });
            AppDispatcher.dispatch({
                actionName: Constants.events.DISPLAY_MESSAGE,
                data: t('resourceOwnerCannotBeUpdated')
            }); 
        });
	}

    /**
    * Change the property.
    */
    handleChangeProperty(e) {
        var self = this;
        var scope = self.state.scope;
        scope[e.target.name] = e.target.value;
        self.state.scope.claims = [];
        self.setState({
            scope: scope
        });
    }

    /**
    * Handle click on the row.
    */
    handleRowClick(e, record) {
        record.isChecked = e.target.checked;
        var claims = this.state.claims;
        this.setState({
            claims: claims
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

    render() {
    	var self = this;
    	const { t, classes } = self.props;
        var claims = [];
        if (self.state.claims) {
            self.state.claims.forEach(function(claim) {
                claims.push(
                    <TableRow key={claim.key}>
                        <TableCell><Checkbox color="primary" checked={claim.isChecked} onChange={(e) => self.handleRowClick(e, claim)} disabled={claim.is_identifier} /></TableCell>
                        <TableCell>{claim.key}</TableCell>
                        <TableCell><Input value={claim.value} onChange={(e) => self.handleChangeClaimValue(e, claim)} disabled={claim.is_identifier} /></TableCell>
                    </TableRow>
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
                    <div style={{float: "right"}}>                        
                        <IconButton onClick={this.saveUser}>
                            <Save />
                        </IconButton>
                    </div>
                </div>
                <div className="body">
                	{ self.state.isLoading ? (<CircularProgress />) : (
                    	<Grid container spacing={40}>
                    		<Grid item md={5} sm={12}>
		                        {/* Login */}
		                        <FormControl fullWidth={true} className={classes.margin} disabled={self.props.isReadOnly}>
		                            <InputLabel>{t('roLogin')}</InputLabel>
		                            <Input value={self.state.user.login} name="login" disabled={true} />
		                            <FormHelperText>{t('roLoginDescription')}</FormHelperText>
		                        </FormControl>                   			
                    		</Grid>
                    		<Grid item md={5} sm={12}>
                    			    {/* Claims */}
                    				<Table>
                    					<TableHead>
                                        	<TableCell></TableCell>
                                        	<TableCell>{t('claimKey')}</TableCell>
                                        	<TableCell>{t('claimValue')}</TableCell>
                    					</TableHead>
                                		<TableBody>
                                            {claims}
                                		</TableBody>
                    				</Table>
                    		</Grid>
                    	</Grid>
                    )}
                </div>
            </div>
    	</div>);
    }

    componentDidMount() {
    	var self = this;
    	self.setState({
    		login: self.props.match.params.id
    	}, function() {
    		self.refreshData();
    	});
    }
}

export default translate('common', { wait: process && !process.release })(withStyles(styles)(ViewUser));