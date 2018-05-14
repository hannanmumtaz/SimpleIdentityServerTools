import React, { Component } from "react";
import { translate } from 'react-i18next';
import { ChipsSelector } from './common';
import { ResourceService } from '../services';
import { NavLink } from 'react-router-dom';
import { withStyles } from 'material-ui/styles';
import Input, { InputLabel } from 'material-ui/Input';
import { FormControl, FormHelperText } from 'material-ui/Form';
import Table, { TableBody, TableCell, TableHead, TableRow, TableFooter } from 'material-ui/Table';
import { CircularProgress, IconButton, Menu, MenuItem, Grid, Chip, Checkbox, Paper, Button, Select } from 'material-ui';
import Dialog, { DialogTitle, DialogContent, DialogActions } from 'material-ui/Dialog';
import MoreVert from '@material-ui/icons/MoreVert';
import Delete from '@material-ui/icons/Delete';
import Save from '@material-ui/icons/Save';
import Collapse from 'material-ui/transitions/Collapse';
import ExpandLess from '@material-ui/icons/ExpandLess';
import ExpandMore from '@material-ui/icons/ExpandMore';
import Edit from '@material-ui/icons/Edit';
import Add from '@material-ui/icons/Add';
import $ from 'jquery';
import { SessionStore } from '../stores';

const styles = theme => ({
  margin: {
    margin: theme.spacing.unit
  }
});

function guid() {
  function s4() {
    return Math.floor((1 + Math.random()) * 0x10000)
      .toString(16)
      .substring(1);
  }
  return s4() + s4() + '-' + s4() + '-' + s4() + '-' + s4() + '-' + s4() + s4() + s4();
}

class ViewResource extends Component {
    constructor(props) {
        super(props);
        this.handleOpenAuthPolicyRuleModal = this.handleOpenAuthPolicyRuleModal.bind(this);
        this.handleCloseAuthPolicyRuleModal = this.handleCloseAuthPolicyRuleModal.bind(this);
        this.handleAddAuthorizationPolicy = this.handleAddAuthorizationPolicy.bind(this);
        this.handleAddAuthRulePolicy = this.handleAddAuthRulePolicy.bind(this);
        this.handleClick = this.handleClick.bind(this);
        this.handleClose = this.handleClose.bind(this);
        this.handleChangeProvider = this.handleChangeProvider.bind(this);
        this.handleProperty = this.handleProperty.bind(this);
        this.handleAllSelections = this.handleAllSelections.bind(this);
        this.handleEditAuthPolicyRule = this.handleEditAuthPolicyRule.bind(this);
        this.handleRemoveAuthPolicyRule = this.handleRemoveAuthPolicyRule.bind(this);
        this.handleRemoveAuthPolicies = this.handleRemoveAuthPolicies.bind(this);
        this.handleSave = this.handleSave.bind(this);
        this.handleDisplayAuthPolicy = this.handleDisplayAuthPolicy.bind(this);
        this.handleRowClick = this.handleRowClick.bind(this);
        this.handleAddClaim = this.handleAddClaim.bind(this);
        this.refreshData = this.refreshData.bind(this);
        this.refreshClaims = this.refreshClaims.bind(this);
        this.state = {
            id: null,
            isLoading: false,
            isRemoveDisplayed: false,
            isAuthPolicyModalOpened: false,
            resourceId: '',
            resourceName: '',
            resourceType: '',
            resourceScopes: [],
            isRemoveDisplayed: false,
            scopeName: '',
            resourceAuthPolicies: [],
            currentAuthPolicy: null,
            currentAuthRulePolicy: {
                provider: '',
                clients: [],
                scopes: [],
                claims: []
            },
            claimType: null,
            claimValue: null,
            supportedClaims: []
        };
    }

    /**
    * Open the modal.
    */
    handleOpenAuthPolicyRuleModal(e) {
        this.setState({
            isAuthPolicyModalOpened: true,
            currentAuthPolicy: e
        });
    }

    /**
    * Close the modal.
    */
    handleCloseAuthPolicyRuleModal() {
        this.setState({
            isAuthPolicyModalOpened: false
        });
    }

    /**
    * Add an authorization policy.
    */
    handleAddAuthorizationPolicy() {
        var resourceAuthPolicies = this.state.resourceAuthPolicies;
        resourceAuthPolicies.push({
            id: guid(),
            rules: [],
            isChecked: false,
            isNew: true
        });
        this.setState({
            resourceAuthPolicies: resourceAuthPolicies
        });
    }

    /**
    * Add the authorization policy rule.
    */
    handleAddAuthRulePolicy() {
        var self = this;
        self.handleCloseAuthPolicyRuleModal();
        var resourceAuthPolicies = self.state.resourceAuthPolicies;
        var currentAuthRulePolicy = self.state.currentAuthRulePolicy;
        var currentAuthPolicy = self.state.currentAuthPolicy;
        currentAuthRulePolicy['consent_needed'] = false;
        currentAuthRulePolicy['id'] = guid();
        currentAuthPolicy.rules.push(currentAuthRulePolicy);
        self.setState({
            resourceAuthPolicies: resourceAuthPolicies,
            currentAuthRulePolicy: {
                provider: '',
                clients: [],
                scopes: [],
                claims: []
            }
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
    * Edit the authorization policy rule.
    */
    handleEditAuthPolicyRule(a, b) {
        this.setState({
            isAuthPolicyModalOpened: true,
            currentAuthPolicy: a,
            currentAuthRulePolicy: b
        });        
    }

    /**
    * Remove the selected authorization policy rule.
    */
    handleRemoveAuthPolicyRule(authPolicy, resourceAuthRulePolicy) {
        const resourceAuthPolicies = this.state.resourceAuthPolicies;
        const resourceAuthPolicyIndex = resourceAuthPolicies.indexOf(authPolicy);
        var resourceAuthPolicy = resourceAuthPolicies[resourceAuthPolicyIndex];
        const resourceAuthRulePolicyIndex = resourceAuthPolicy.rules.indexOf(resourceAuthRulePolicy);
        resourceAuthPolicy.rules.splice(resourceAuthRulePolicyIndex, 1);
        this.setState({
            resourceAuthPolicies: resourceAuthPolicies
        });
    }

    /**
    * Add a claim.
    */
    handleAddClaim() {
        var self = this;
        var claimType = self.state.claimType;
        var claimValue = self.state.claimValue;
        var currentAuthRulePolicy = self.state.currentAuthRulePolicy;
        var currentAuthPolicyClaimTypes = currentAuthRulePolicy.claims.map(function(c) { return c.type; });
        if (currentAuthPolicyClaimTypes.indexOf(claimType) !== -1 || !claimType || claimType === '' || !claimValue || claimValue === '') {
            return;
        }

        currentAuthRulePolicy.claims.push({
            type: claimType,
            value: claimValue
        });
        self.setState({
            claimValue: '',
            currentAuthRulePolicy: currentAuthRulePolicy
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
    * Change the provider.
    */
    handleChangeProvider(e) {
        var currentAuthRulePolicy = this.state.currentAuthRulePolicy;
        currentAuthRulePolicy['provider'] = e.target.value;
        this.setState({
            currentAuthRulePolicy: currentAuthRulePolicy
        });
    }

    /**
    * Handle property change.
    */
    handleProperty(e)  {
        this.setState({
            [e.target.name]: e.target.value
        });
    }

    /**
    * Select all the rows.
    */
    handleAllSelections(e) {
        var checked = e.target.checked;
        var data = this.state.resourceAuthPolicies;
        data.forEach(function(r) { r.isChecked = checked ;});
        this.setState({
            resourceAuthPolicies: data,
            isRemoveDisplayed: checked
        });
    }
    /**
    * Remove the selected authorization policies.
    */
    handleRemoveAuthPolicies() {
        var resourceAuthPolicies = this.state.resourceAuthPolicies.filter(function(auth) { return !auth.isChecked; }); 
        this.setState({
            resourceAuthPolicies: resourceAuthPolicies
        });
    }

    /**
    * Save the changes.
    */
    handleSave() {
        var self = this;
        var resourceId = self.state.resourceId;
        var resourceName = self.state.resourceName;
        var resourceType = self.state.resourceType;
        var resourceScopes = self.state.resourceScopes;        
        var resourceAuthPolicies = self.state.resourceAuthPolicies;
        var rules = [];
        var resourceRequest = {
            _id: resourceId,
            name: resourceName,
            scopes : resourceScopes,
            type: resourceType
        };
        var insertAuthPolicyRequest = [],
            updateAuthPolicyRequest = [];
        resourceAuthPolicies.forEach(function(resourceAuthPolicy) {
            if (resourceAuthPolicy.isNew) {
                resourceAuthPolicy['resource_set_ids'] =  [ resourceId ];
                insertAuthPolicyRequest.push(resourceAuthPolicy);
            } else {
                updateAuthPolicyRequest.push(resourceAuthPolicy);
            }
        });

        console.log(insertAuthPolicyRequest);
        console.log(updateAuthPolicyRequest);
    }

    /**
    * Toggle the authorization policy.
    */
    handleDisplayAuthPolicy(authPolicy, isDeployed) {
        authPolicy.isDeployed = isDeployed;
        var resourceAuthPolicies = this.state.resourceAuthPolicies;
        this.setState({
            resourceAuthPolicies: resourceAuthPolicies
        });
    }

    /**
    * Handle click on the row.
    */
    handleRowClick(e, record) {
        record.isChecked = e.target.checked;
        var data = this.state.resourceAuthPolicies;
        var nbSelectedRecords = data.filter(function(r) { return r.isChecked; }).length;
        this.setState({
            resourceAuthPolicies: data,
            isRemoveDisplayed: nbSelectedRecords > 0
        });
    }


    /**
    * Display the resource.
    */
    refreshData() {
        var self = this;
        self.setState({
            isLoading: true
        });
        Promise.all([ ResourceService.get(self.state.id), ResourceService.getAuthPolicies(self.state.id) ]).then(function(values) {
            var resource = values[0];
            var policies = values[1];
            var p = [];
            if (policies.content) {
                policies.content.forEach(function(policy) {
                    p.push({
                        id: policy.id,
                        rules: policy.rules,
                        isChecked: false,
                        isDeployed: false,
                        isNew: false
                    });
                });
            }

            self.setState({
                resourceId: resource._id,
                resourceName: resource.name,
                resourceScopes: resource.scopes,
                resourceAuthPolicies: p,
                isLoading: false,
            });
        }).catch(function() {            
            console.log(e);
            self.setState({
                isLoading: false
            });
        });
    }

    /**
    * Refresh the claims.
    */
    refreshClaims() {
        var self = this;
        var profile = SessionStore.getSession();
        if (!profile.openid_url) {
            return;
        }

        $.get(profile.openid_url).then(function(r) {
            var claimsSupported = r['claims_supported'];
            self.setState({
                supportedClaims: claimsSupported,
                claimType: claimsSupported[0]
            });
        }).fail(function() {
            self.setState({
                supportedClaims: [ ]
            });
        });
    }

    /**
    * Remove the claim.
    */
    handleRemoveClaim(claim) {
        const currentAuthRulePolicy = this.state.currentAuthRulePolicy;
        const claims = currentAuthRulePolicy.claims;
        const claimIndex = claims.indexOf(claim);
        claims.splice(claimIndex, 1);        
        this.setState({
            currentAuthRulePolicy: currentAuthRulePolicy
        });
    }

    render() {
        var self = this;
        const { t, classes } = self.props;
        var rows = [],
            claims = [],
            chips = [];
        if (self.state.resourceAuthPolicies) {
            self.state.resourceAuthPolicies.forEach(function(authPolicy) {
                var authPolicies = [];
                if (authPolicy.rules) {
                    authPolicy.rules.forEach(function(rule) {
                        authPolicies.push(
                            <TableRow>
                                <TableCell>{rule.clients.join(',')}</TableCell>
                                <TableCell>{rule.claims.map(function(r) { return r.type + ":" + r.value }).join(',')}</TableCell>
                                <TableCell>{rule.scopes.join(',')}</TableCell>
                                <TableCell>{rule.provider}</TableCell>
                                <TableCell>
                                    <IconButton onClick={() => { self.handleEditAuthPolicyRule(authPolicy, rule); }}>
                                        <Edit />
                                    </IconButton>
                                    <IconButton onClick={() => { self.handleRemoveAuthPolicyRule(authPolicy, rule); }}>
                                        <Delete />
                                    </IconButton>
                                </TableCell>
                            </TableRow>
                        );
                    });
                }


                rows.push(
                    <TableRow key={authPolicy.id}>
                        <TableCell><Checkbox color="primary" checked={authPolicy.isChecked} onChange={(e) => self.handleRowClick(e, authPolicy)} /></TableCell>
                        <TableCell>
                            {t('authorizationPolicy') +": " + authPolicy.id}
                            <IconButton onClick={() => self.handleOpenAuthPolicyRuleModal(authPolicy)}>
                                <Add />
                            </IconButton>
                            { authPolicy.isDeployed ? (<IconButton onClick={() => self.handleDisplayAuthPolicy(authPolicy, false)}><ExpandLess /> </IconButton>) : (<IconButton onClick={() => self.handleDisplayAuthPolicy(authPolicy, true)}><ExpandMore /></IconButton>) }                            
                            <Collapse in={authPolicy.isDeployed}>
                                <Table>
                                    <TableHead>
                                        <TableRow>
                                            <TableCell>{t('allowedClients')}</TableCell>
                                            <TableCell>{t('allowedClaims')}</TableCell>
                                            <TableCell>{t('scopes')}</TableCell>
                                            <TableCell>{t('authProvider')}</TableCell>
                                            <TableCell></TableCell>
                                        </TableRow>
                                    </TableHead>
                                    <TableBody>
                                        {authPolicies}
                                    </TableBody>
                                </Table>
                            </Collapse> 
                        </TableCell>
                    </TableRow>
                );
            });
        }

        if (self.state.supportedClaims) {
            self.state.supportedClaims.forEach(function(claim) {
                claims.push((<MenuItem key={claim} value={claim}>{t(claim)}</MenuItem>));
            });
        }

        if (self.state.currentAuthRulePolicy.claims) {
            self.state.currentAuthRulePolicy.claims.forEach(function(claim) {
                chips.push((<Chip label={claim.type + " : " + claim.value} key={claim.type} className={classes.margin} onDelete={() => self.handleRemoveClaim(claim)} />));
            });
        }

        return (<div className="block">
            <Dialog open={self.state.isAuthPolicyModalOpened} onClose={this.handleCloseAuthPolicyRuleModal}>
                <DialogTitle>{t('addAuthRulePolicy')}</DialogTitle>
                {self.state.isAddUserLoading ? (<CircularProgress />) : (
                    <div>
                        <DialogContent>
                            {/* Provider */}
                            <FormControl fullWidth={true}>
                                <InputLabel htmlFor="provider">{t('provider')}</InputLabel>
                                <Input id="provider" value={self.state.currentAuthRulePolicy.provider} name="provider" onChange={self.handleChangeProvider}  />
                            </FormControl>
                            {/* AllowedClients */}
                            <FormControl fullWidth={true}>
                                <ChipsSelector label={t('allowedClients')} properties={self.state.currentAuthRulePolicy.clients} />
                            </FormControl>
                            {/* Allowed claims */}
                            <FormControl fullWidth={true}>
                                <form onSubmit={(e) => { e.preventDefault(); self.handleAddClaim(); }}>
                                    <Select value={self.state.claimType} onChange={self.handleProperty} name="claimType">
                                        {claims}
                                    </Select>                                    
                                    <FormControl className={classes.margin}>
                                        <InputLabel>{t('claimValue')}</InputLabel>
                                        <Input value={self.state.claimValue} name="claimValue" onChange={self.handleProperty}  />
                                    </FormControl>
                                    <Button variant="raised" color="primary" onClick={this.handleAddClaim}>{t('add')}</Button>
                                </form>
                                <Paper>                                    
                                    {chips}
                                </Paper>
                            </FormControl>
                            {/* Allowed scopes */}
                            <FormControl fullWidth={true}>
                                <ChipsSelector label={t('allowedScopes')} properties={self.state.currentAuthRulePolicy.scopes} />
                            </FormControl>
                        </DialogContent>
                        <DialogActions>
                            <Button  variant="raised" color="primary" onClick={self.handleAddAuthRulePolicy}>{t('save')}</Button>
                        </DialogActions>
                    </div>
                )}
            </Dialog>
            <div className="block-header">
                <Grid container>
                    <Grid item md={5} sm={12}>
                        <h4>{t('resourceTitle')}</h4>
                        <i>{t('resourceShortDescription')}</i>
                    </Grid>
                    <Grid item md={7} sm={12}>                        
                        <ul className="breadcrumb float-md-right">
                            <li className="breadcrumb-item"><NavLink to="/">{t('websiteTitle')}</NavLink></li>
                            <li className="breadcrumb-item"><NavLink to="/resources">{t('resources')}</NavLink></li>
                            <li className="breadcrumb-item">{t('resource')}</li>
                        </ul>
                    </Grid>
                </Grid>
            </div>
            <div className="card">
                { self.state.isLoading ? ( <CircularProgress /> ) : (
                    <div>
                        <div className="header">
                            <h4 style={{display: "inline-block"}}>{t('resourceInformation')}</h4>
                            <div style={{float: "right"}}>
                                <IconButton onClick={self.handleAddAuthorizationPolicy}>
                                    <Add />
                                </IconButton>
                                {self.state.isRemoveDisplayed && (
                                    <IconButton onClick={self.handleRemoveAuthPolicies}>
                                        <Delete />
                                    </IconButton>
                                )}
                                <IconButton onClick={self.handleSave}>
                                    <Save />
                                </IconButton>
                            </div>
                        </div>
                        <div className="body">
                            <Grid container spacing={40}>
                                <Grid item sm={12} md={4}>
                                    {/* Id */}
                                    <FormControl className={classes.margin} fullWidth={true}>
                                        <InputLabel>{t('resourceId')}</InputLabel>
                                        <Input value={self.state.resourceId}  disabled={true}  />
                                    </FormControl>
                                    {/* Name */}
                                    <FormControl className={classes.margin} fullWidth={true}>
                                        <InputLabel htmlFor="resourceName">{t('resourceName')}</InputLabel>
                                        <Input id="resourceName" value={self.state.resourceName} name="resourceName" onChange={self.handleProperty}  />
                                    </FormControl>
                                    {/* Type */}
                                    <FormControl className={classes.margin} fullWidth={true}>
                                        <InputLabel htmlFor="resourceType">{t('resourceType')}</InputLabel>
                                        <Input id="resourceType" value={self.state.resourceType} name="resourceType" onChange={self.handleProperty}  />
                                    </FormControl>
                                    {/* Scopes */}
                                    <div className={classes.margin}>
                                        <ChipsSelector label={t('resourceScopes')} properties={self.state.resourceScopes} />
                                    </div>
                                </Grid>
                                <Grid item sm={12} md={8}>
                                    <Table>
                                        <TableHead>
                                            <TableRow>
                                                <TableCell><Checkbox color="primary" onChange={self.handleAllSelections} /></TableCell>
                                                <TableCell></TableCell>
                                            </TableRow>
                                        </TableHead>
                                        <TableBody>
                                            {rows}
                                        </TableBody>
                                    </Table>
                                </Grid>                                
                            </Grid>
                        </div>
                    </div>
                )}
            </div>
        </div>);
    }

    componentDidMount() {
        var self = this;
        self.setState({
            id:  self.props.match.params.id
        }, () => {
            SessionStore.addChangeListener(function() {
                self.refreshClaims();
            });
            self.refreshData();
            self.refreshClaims();
        });
    }
}

export default translate('common', { wait: process && !process.release })(withStyles(styles)(ViewResource));