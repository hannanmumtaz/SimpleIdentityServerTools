import React, { Component } from "react";
import { translate } from 'react-i18next';
import { Grid, CircularProgress, IconButton } from 'material-ui';
import { NavLink } from 'react-router-dom';
import Constants from '../constants';
import { AccountFilterService } from '../services';
import { SessionStore } from '../stores';
import Input, { InputLabel } from 'material-ui/Input';
import { FormControl, FormHelperText } from 'material-ui/Form';
import Save from '@material-ui/icons/Save';

class ViewAccountFilter extends Component {
    constructor(props) {
        super(props);
        this.refreshData = this.refreshData.bind(this);
        this.saveAccountFilter = this.saveAccountFilter.bind(this);
        this.state = {
            isLoading: true,
            id: null,
            accountFilter: {}
        };
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

    }

    render() {
        var self = this;
        const { t } = self.props;
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
                                    <Input type="text" value={self.state.accountFilter.name} />
                                    <FormHelperText>{t('accountFilterNameDescription')}</FormHelperText>
                                </FormControl>
                            </Grid>
                            <Grid item md={6} sm={12}>
                                
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