import React, { Component } from "react";
import { translate } from 'react-i18next';
import { NavLink } from 'react-router-dom';
import { ClaimService } from '../services';
import { withStyles } from 'material-ui/styles';
import { CircularProgress, IconButton, Select, MenuItem, Checkbox, Typography, Grid } from 'material-ui';
import { FormControl, FormHelperText } from 'material-ui/Form';
import Input, { InputLabel } from 'material-ui/Input';
import { DisplayScope } from './common';
import { SessionStore } from '../stores';
import Tabs, { Tab } from 'material-ui/Tabs';
import Save from '@material-ui/icons/Save';
import AppDispatcher from '../appDispatcher';
import Constants from '../constants';
import moment from 'moment';


const styles = theme => ({
  margin: {
    margin: theme.spacing.unit,
  }
});

class ViewClaim extends Component {
    constructor(props) {
        super(props);
        this.refreshData = this.refreshData.bind(this);
        this.state = {
            isLoading: true,
            id: null,
            claim: {}
        };
    }

    /**
    * Display the scope.
    */
    refreshData() {
        var self = this;
        self.setState({
            isLoading: true
        });
        ClaimService.get(self.state.id).then(function(claim) {
            self.setState({
                isLoading: false,
                claim: claim
            });
        }).catch(function() {
            self.setState({
                isLoading: false
            });
        });
    }

    /**
    * Display the view.
    */
    render() {
        var self = this;
        const { t, classes } = self.props;
        return (<div className="block">
            <div className="block-header">
                <Grid container>
                    <Grid item md={5} sm={12}>
                        <h4>{t('claimTitle')}</h4>
                        <i>{t('claimShortDescription')}</i>
                    </Grid>
                    <Grid item md={7} sm={12}>                        
                        <ul className="breadcrumb float-md-right">
                            <li className="breadcrumb-item"><NavLink to="/">{t('websiteTitle')}</NavLink></li>
                            <li className="breadcrumb-item"><NavLink to="/claims">{t('claims')}</NavLink></li>
                            <li className="breadcrumb-item">{t('claim')}</li>
                        </ul>
                    </Grid>
                </Grid>
            </div>
            <div className="card">
                <div className="header">
                    <h4 style={{display: "inline-block"}}>{t('claimInformation')}</h4>
                </div>
                <div className="body">
                    { self.state.isLoading ? (<CircularProgress />) : (                        
                        <Grid container spacing={40}>
                            <Grid item sm={12} md={6}>
                                {/* Code */}
                                <FormControl fullWidth={true} className={classes.margin} disabled={true}>
                                    <InputLabel>{t('claimCode')}</InputLabel>
                                    <Input value={self.state.claim.key} />
                                </FormControl>
                                <FormControl fullWidth={true} className={classes.margin} disabled={true}>
                                    <InputLabel>{t('updateDateTime')}</InputLabel>
                                    <Input value={moment(self.state.claim.update_datetime).format('LLLL')} />
                                </FormControl>
                                {/* Is used as identifier */ }
                                <Typography><Checkbox color="primary" checked={self.state.claim.is_identifier} disabled={true}/> {t('isUsedAsOpenidIdentifier')}</Typography >
                            </Grid>
                        </Grid>
                    ) }
                </div>
            </div>
        </div>);
    }

    componentDidMount() {
        var self = this;
        SessionStore.addChangeListener(function() {
            self.setState({
                id : self.props.match.params.id
            }, function() {
                self.refreshData();
            });
        });
        if (SessionStore.getSession().selectedOpenid) {
            self.setState({
                id : self.props.match.params.id
            }, function() {
                self.refreshData();
            });
        }
    }
}

export default translate('common', { wait: process && !process.release })(withStyles(styles)(ViewClaim));