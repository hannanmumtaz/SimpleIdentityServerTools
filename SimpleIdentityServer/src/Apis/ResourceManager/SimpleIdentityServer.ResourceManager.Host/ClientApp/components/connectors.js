import React, { Component } from "react";
import { translate } from 'react-i18next';
import { NavLink } from 'react-router-dom';
import { ParameterService } from '../services';
import AppDispatcher from '../appDispatcher';
import Constants from '../constants';

import { Popover, IconButton, Menu, Divider, Paper, Input, MenuItem, Checkbox, TextField, Select, Avatar , CircularProgress, Grid, Button, ExpansionPanel, ExpansionPanelSummary, ExpansionPanelDetails, Typography } from 'material-ui';

class Connectors extends Component {
    constructor(props) {
        super(props);
        this.refreshData = this.refreshData.bind(this);
        this.state = {
            isLoading: true
        };
    }

    refreshData() {
        var self = this;
        const {t} = self.props;
        self.setState({
            isLoading: true
        });
        ParameterService.getConnectors().then(function(result) {
            console.log(result);
            self.setState({
                isLoading: false
            });
        }).catch(function() {
            AppDispatcher.dispatch({
                actionName: Constants.events.DISPLAY_MESSAGE,
                data: t('connectorsCannotBeRetrieved')
            });
            self.setState({
                isLoading: false
            });
        });
    }

    render() {
        var self = this;
        const { t } = self.props;
        return (<div className="block">
            <div className="block-header">
                <Grid container>
                    <Grid item md={5} sm={12}>
                        <h4>{t('connectorsTitle')}</h4>
                        <i>{t('connectorsShortDescription')}</i>
                    </Grid>
                    <Grid item md={7} sm={12}>                        
                        <ul className="breadcrumb float-md-right">
                            <li className="breadcrumb-item"><NavLink to="/">{t('websiteTitle')}</NavLink></li>
                            <li className="breadcrumb-item">{t('connectors')}</li>
                        </ul>
                    </Grid>
                </Grid>
            </div>
            <div className="card">
                <div className="body">
                    { self.state.isLoading ? (<CircularProgress />) : (
                        <span>{t('connectors')}</span>
                    )}
                </div>
            </div>
        </div>);
    }

    componentDidMount() {
        this.refreshData();
    }
}

export default translate('common', { wait: process && !process.release })(Connectors);