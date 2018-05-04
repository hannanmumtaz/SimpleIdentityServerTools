import React, { Component } from "react";
import { translate } from 'react-i18next';
import { ChipsSelector } from './common';
import { ResourceService } from '../services';
import { NavLink } from 'react-router-dom';
import { withStyles } from 'material-ui/styles';
import Input, { InputLabel } from 'material-ui/Input';
import { FormControl, FormHelperText } from 'material-ui/Form';
import Table, { TableBody, TableCell, TableHead, TableRow, TableFooter } from 'material-ui/Table';
import { CircularProgress, IconButton, Menu, MenuItem, Grid, Chip, Checkbox, Paper, Button } from 'material-ui';
import MoreVert from '@material-ui/icons/MoreVert';
import Delete from '@material-ui/icons/Delete';
import Save from '@material-ui/icons/Save';
import Collapse from 'material-ui/transitions/Collapse';
import ExpandLess from '@material-ui/icons/ExpandLess';
import ExpandMore from '@material-ui/icons/ExpandMore';

const styles = theme => ({
  margin: {
    margin: theme.spacing.unit
  }
});

class ViewResource extends Component {
    constructor(props) {
        super(props);
        this.handleClick = this.handleClick.bind(this);
        this.handleClose = this.handleClose.bind(this);
        this.handleProperty = this.handleProperty.bind(this);
        this.handleAllSelections = this.handleAllSelections.bind(this);
        this.handleRemoveAuthPolicies = this.handleRemoveAuthPolicies.bind(this);
        this.handleSave = this.handleSave.bind(this);
        this.handleDisplayAuthPolicy = this.handleDisplayAuthPolicy.bind(this);
        this.handleRowClick = this.handleRowClick.bind(this);
        this.refreshData = this.refreshData.bind(this);
        this.state = {
            id: null,
            resourceName: '',
            resourceType: '',
            resourceScopes: [],
            isLoading: false,
            isRemoveDisplayed: false,
            scopeName: '',
            resourceAuthPolicies: []
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

    }

    /**
    * Save the changes.
    */
    handleSave() {

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
                        isDeployed: false
                    });
                });
            }

            self.setState({
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

    render() {
        var self = this;
        const { t, classes } = self.props;
        var rows = [];
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
                            </TableRow>
                        );
                    });
                }


                rows.push(
                    <TableRow key={authPolicy.id}>
                        <TableCell><Checkbox color="primary" checked={authPolicy.isChecked} onChange={(e) => self.handleRowClick(e, authPolicy)} /></TableCell>
                        <TableCell>
                            {t('authorizationPolicy') +": " + authPolicy.id}
                            { authPolicy.isDeployed ? (<IconButton onClick={() => self.handleDisplayAuthPolicy(authPolicy, false)}><ExpandLess /> </IconButton>) : (<IconButton onClick={() => self.handleDisplayAuthPolicy(authPolicy, true)}><ExpandMore /></IconButton>) }                            
                            <Collapse in={authPolicy.isDeployed}>
                                <Table>
                                    <TableHead>
                                        <TableRow>
                                            <TableCell>{t('allowedClients')}</TableCell>
                                            <TableCell>{t('allowedClaims')}</TableCell>
                                            <TableCell>{t('scopes')}</TableCell>
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

        console.log(classes);
        return (<div className="block">
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
                                <IconButton onClick={self.handleSave}>
                                    <Save />
                                </IconButton>
                                {self.state.isRemoveDisplayed && (
                                    <IconButton onClick={self.handleRemoveAuthPolicies}>
                                        <Delete />
                                    </IconButton>
                                )}
                                <IconButton onClick={this.handleClick}>
                                    <MoreVert />
                                </IconButton>
                                <Menu anchorEl={self.state.anchorEl} open={Boolean(self.state.anchorEl)} onClose={self.handleClose}>
                                    <MenuItem>{t('addAuthPolicy')}</MenuItem>
                                </Menu>
                            </div>
                        </div>
                        <div className="body">
                            <Grid container spacing={40}>
                                <Grid item sm={12} md={4}>
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
            self.refreshData();
        });
    }
}

export default translate('common', { wait: process && !process.release })(withStyles(styles)(ViewResource));