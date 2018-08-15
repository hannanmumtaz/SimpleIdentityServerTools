import React, { Component } from "react";
import { translate } from 'react-i18next';
import { Grid, CircularProgress, IconButton, Select, MenuItem, Button, List, ListItem, ListItemText } from 'material-ui';
import { NavLink } from 'react-router-dom';
import AppDispatcher from '../appDispatcher';
import Constants from '../constants';
import { AccountFilterService } from '../services';
import { SessionStore } from '../stores';
import Input, { InputLabel } from 'material-ui/Input';
import { FormControl, FormHelperText } from 'material-ui/Form';
import Save from '@material-ui/icons/Save';
import Delete from '@material-ui/icons/Delete';

class ViewAccountFilter extends Component {
    constructor(props) {
        super(props);
        this.handleRemoveRule = this.handleRemoveRule.bind(this);
        this.refreshData = this.refreshData.bind(this);
        this.saveAccountFilter = this.saveAccountFilter.bind(this);
        this.handleChangeFilterProperty = this.handleChangeFilterProperty.bind(this);
        this.handleChangeFilterRuleProperty = this.handleChangeFilterRuleProperty.bind(this);
        this.handleAddFilterRule = this.handleAddFilterRule.bind(this);
        this.state = {
            isLoading: true,
            id: null,
            accountFilter: {},
            newFilterRule: {
                claim_key: '',
                claim_value: '',
                op: "eq"
            }
        };
    }

    /**
    * Remove the rule
    */
    handleRemoveRule(rule) {
        var self = this;
        var accountFilter = self.state.accountFilter;
        var index = accountFilter.rules.indexOf(rule);
        accountFilter.rules.splice(index, 1);
        self.setState({
            accountFilter: accountFilter
        });
    }

    /**
    * Refresh the data.
    */
    refreshData() {
        var self = this;
        self.setState({
            isLoading: true
        });
        AccountFilterService.getAccountFilter(self.state.id).then(function(result) {
            self.setState({
                isLoading: false,
                accountFilter: result
            });
        }).catch(function() {
            AppDispatcher.dispatch({
                actionName: Constants.events.DISPLAY_MESSAGE,
                data: t('cannotRetrievedAccountFilter')
            });
            self.setState({
                isLoading: false,
                accountFilter: {}
            });
        });
    }

    /**
    * Save the account filter.
    */
    saveAccountFilter() {
        var self = this;
        self.setState({
            isLoading: true
        });
        const { t } = self.props;
        var request = self.state.accountFilter;
        request['id'] = self.state.id;
        AccountFilterService.updateAccountFilter(request).then(function() {
            AppDispatcher.dispatch({
                actionName: Constants.events.DISPLAY_MESSAGE,
                data: t('accountFilterUpdated')
            });
            self.refreshData();
        }).catch(function() {
            AppDispatcher.dispatch({
                actionName: Constants.events.DISPLAY_MESSAGE,
                data: t('cannotUpdateAccountFilter')
            });
            self.setState({
                isLoading: false
            });
        });
    }

    handleChangeFilterProperty(e) {
        var self = this;
        var filter = self.state.accountFilter;
        filter[e.target.name] = e.target.value;
        self.setState({
            accountFilter: filter
        });
    }

    /**
    * Handle change property
    */
    handleChangeFilterRuleProperty(e) {        
        var self = this;
        var newFilterRule = self.state.newFilterRule;
        newFilterRule[e.target.name] = e.target.value;
        self.setState({
            newFilterRule: newFilterRule
        });
    }

    /**
    * Add the filter rule.
    */
    handleAddFilterRule() {
        var self = this;
        var accountFilter = self.state.accountFilter;
        var newFilterRule = self.state.newFilterRule;
        if (!newFilterRule.claim_key || newFilterRule.claim_key === '' || !newFilterRule.claim_value || newFilterRule.claim_value === '')
        {
            return;
        }

        accountFilter.rules.push(newFilterRule);
        self.setState({
            accountFilter: accountFilter,
            newFilterRule: {
                claim_key: '',
                claim_value: '',
                op: "eq"
            }
        });
    }

    render() {
        var self = this;
        const { t } = self.props;
        var rules = [];
        if (self.state.accountFilter.rules) {
            self.state.accountFilter.rules.forEach(function(rule) {
                rules.push(
                    <ListItem dense button style={{overflow: 'hidden'}}>
                        <IconButton onClick={() => self.handleRemoveRule(rule)}>
                            <Delete />
                        </IconButton>
                        <ListItemText>{rule.claim_key} {rule.op} {rule.claim_value}</ListItemText>
                    </ListItem>
                );
            });
        }

        return (<div className="block">
            <div className="block-header">
                <Grid container>
                    <Grid item md={5} sm={12}>
                        <h4>{t('accountFilterTitle')}</h4>
                        <i>{t('accountFilterShortDescription')}</i>
                    </Grid>
                    <Grid item md={7} sm={12}>                        
                        <ul className="breadcrumb float-md-right">
                            <li className="breadcrumb-item"><NavLink to="/">{t('websiteTitle')}</NavLink></li>
                            <li className="breadcrumb-item">
                                <NavLink to="/openid/accountfilters">
                                    {t('accountFilters')}
                                </NavLink>
                            </li>
                            <li className="breadcrumb-item">{t('accountFilter')}</li>
                        </ul>
                    </Grid>
                </Grid>
            </div>
            <div className="card">
                <div className="header">                    
                    <h4 style={{display: "inline-block"}}>{t('accountFilter')}</h4>                    
                    <div style={{float: "right"}}>                        
                        <IconButton onClick={this.saveAccountFilter}>
                            <Save />
                        </IconButton>
                    </div>
                </div>
                <div className="body">
                    { self.state.isLoading ? (<CircularProgress />) : (
                        <Grid container spacing={40}>
                            <Grid item md={6} sm={12}>
                                {/* Account filter id */}
                                <FormControl fullWidth={true} style={{margin: "5px"}} disabled={true}>
                                    <InputLabel htmlFor="accountFilterId">{t('accountFilterId')}</InputLabel>
                                    <Input type="text" value={self.state.accountFilter.id} />
                                </FormControl>
                                {/* Account filter name */}
                                <FormControl fullWidth={true} style={{margin: "5px"}}>
                                    <InputLabel htmlFor="accountFilterName">{t('accountFilterName')}</InputLabel>
                                    <Input type="text" name="name" onChange={self.handleChangeFilterProperty} value={self.state.accountFilter.name}  />
                                    <FormHelperText>{t('accountFilterNameDescription')}</FormHelperText>
                                </FormControl>
                            </Grid>
                            <Grid item md={6} sm={12}>
                                <div>
                                    {/* Claim key */}
                                    <FormControl fullWidth={true} style={{margin: "5px"}}>
                                        <InputLabel htmlFor="claimKey">{t('claimKey')}</InputLabel> 
                                        <Input type="text" name="claim_key" onChange={self.handleChangeFilterRuleProperty} value={self.state.newFilterRule.claim_key}  />                                    
                                        <FormHelperText>{t('claimKeyDescription')}</FormHelperText>                                   
                                    </FormControl>
                                    {/* Claim value */}
                                    <FormControl fullWidth={true} style={{margin: "5px"}}>
                                        <InputLabel htmlFor="claimValue">{t('claimValue')}</InputLabel> 
                                        <Input type="text" name="claim_value" onChange={self.handleChangeFilterRuleProperty} value={self.state.newFilterRule.claim_value}  />                                    
                                        <FormHelperText>{t('claimValueDescription')}</FormHelperText>                                   
                                    </FormControl>
                                    {/* Operation */}
                                    <FormControl fullWidth={true} style={{margin: "5px"}}>
                                        <InputLabel htmlFor="filterRuleOperation">{t('filterRuleOperation')}</InputLabel>     
                                        <Select value={self.state.newFilterRule.op} onChange={self.handleChangeFilterRuleProperty} name="op">
                                            <MenuItem value="eq">{t('equal')}</MenuItem>
                                            <MenuItem value="neq">{t('notEqual')}</MenuItem>
                                            <MenuItem value="regex">{t('regex')}</MenuItem>
                                        </Select>
                                        <FormHelperText>{t('filterRuleOperationDescription')}</FormHelperText>
                                    </FormControl>
                                    {/* Submit button */}
                                    <Button variant="raised" color="primary" onClick={self.handleAddFilterRule}>{t('addFilterRule')}</Button>
                                </div>
                                <List>
                                    {rules}
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
                id: self.props.match.params.id,
            }, function() {
                self.refreshData();
            });
        });
        
        if (SessionStore.getSession().selectedOpenid) {
            self.setState({
                id: self.props.match.params.id
            }, function() {
                self.refreshData();
            });
        }
    }
}

export default translate('common', { wait: process && !process.release })(ViewAccountFilter);