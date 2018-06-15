import React, { Component } from "react";
import { translate } from 'react-i18next';
import { NavLink } from 'react-router-dom';
import { withStyles } from 'material-ui/styles';
import { ParameterService } from '../services';
import AppDispatcher from '../appDispatcher';
import Constants from '../constants';
import $ from 'jquery';

import { Popover, IconButton, Menu, Divider, Paper, MenuItem, Checkbox, TextField, Select, Avatar , CircularProgress, Grid, Button, ExpansionPanel, ExpansionPanelSummary, ExpansionPanelDetails, Typography, Switch } from 'material-ui';
import Dialog, { DialogTitle, DialogContent, DialogActions } from 'material-ui/Dialog';
import Input, { InputLabel } from 'material-ui/Input';
import { FormControl, FormHelperText } from 'material-ui/Form';
import Save from '@material-ui/icons/Save';
import Edit from '@material-ui/icons/Edit'; 

const styles = theme => ({
  margin: {
    margin: theme.spacing.unit,
  }
});


class Connectors extends Component {
    constructor(props) {
        super(props);
        this.refreshData = this.refreshData.bind(this);
        this.handleSwitch = this.handleSwitch.bind(this);
        this.handleEdit = this.handleEdit.bind(this);
        this.handleCloseModal = this.handleCloseModal.bind(this);
        this.handleChangeProperty = this.handleChangeProperty.bind(this);
        this.handleSaveSettings = this.handleSaveSettings.bind(this);
        this.handleSave = this.handleSave.bind(this);
        this.state = {
            isLoading: true,
            isModalOpened: false,
            isSavingConnectorSettings: false,
            selectedConnector: {},
            selectedParameters: {},
            connectors: []
        };
    }

    /**
    * Display the connectors.
    */
    refreshData() {
        var self = this;
        const {t} = self.props;
        self.setState({
            isLoading: true
        });
        ParameterService.getConnectors().then(function(result) {
            var connectors = result['template_connectors'];
            var activeConnectors = result['connectors'];
            if (connectors) {
                connectors.forEach(function(connector) {
                    if (!activeConnectors) {
                        connector['is_enabled'] = false;
                        return;
                    }

                    var filteredConnectors = activeConnectors.filter(function(ac) { return ac.name ===  connector.name});
                    if (filteredConnectors && filteredConnectors.length > 0) {
                        connector['is_enabled'] = true;
                        connector['parameters'] = filteredConnectors[0]['parameters'];
                    } else {
                        connector['is_enabled'] = false;
                    }
                });
            }

            self.setState({
                isLoading: false,
                connectors: connectors
            });
        }).catch(function() {
            AppDispatcher.dispatch({
                actionName: Constants.events.DISPLAY_MESSAGE,
                data: t('connectorsCannotBeRetrieved')
            });
            self.setState({
                isLoading: false,
                connectors: []
            });
        });
    }

    /**
    *  Handle the switch button.
    */
    handleSwitch(connector) {
        var self = this;
        var connectors = self.state.connectors;
        var selectedConnector = connectors.filter(function(co) { return co.name === connector.name; })[0];
        selectedConnector['is_enabled'] = !selectedConnector['is_enabled'];
        self.setState({
            connectors: connectors
        });
    }

    /**
    * Handle edit the connector.
    */
    handleEdit(connector) {
        var self = this;
        var selectedParameters = $.extend({}, connector.parameters);
        self.setState({
            isModalOpened: true,
            selectedConnector: connector,
            selectedParameters: selectedParameters
        });
    }

    /**
    * Close the modal.
    */
    handleCloseModal() {
        this.setState({
            isModalOpened: false,
            selectedParameters: {},
            selectedConnector: {},
        });
    }

    /**
    * Change the property value.
    */
    handleChangeProperty(e) {
        var self = this;
        var selectedParameters = self.state.selectedParameters;
        selectedParameters[e.target.name] = e.target.value;
        self.setState({
            selectedParameters: selectedParameters
        });
    }

    /**
    * Save the settings.
    */
    handleSaveSettings() {
        var self = this;
        const {t} = self.props;
        var selectedParameters = self.state.selectedParameters;
        for(var parameterName in selectedParameters) {
            var parameterValue = selectedParameters[parameterName];
            if(!parameterValue || parameterValue === '') {
                AppDispatcher.dispatch({
                    actionName: Constants.events.DISPLAY_MESSAGE,
                    data: t('allTheParametersMustBeFilledIn')
                });
                return;
            }
        }

        var connectors = self.state.connectors;
        var selectedConnector = connectors.filter(function(co) { return co.name === self.state.selectedConnector.name; })[0];
        selectedConnector.parameters = selectedParameters;
        self.setState({
            isModalOpened: false,
            selectedParameters: {},
            selectedConnector: {},
            connectors: connectors
        });
    }

    /**
    * Save the settings.
    */
    handleSave() {
        var self = this;
        const { t } = self.props;
        self.setState({
            isLoading: true
        });
        var connectors = self.state.connectors.filter(function(connector) { return connector['is_enabled'] === true; }).map(function(connector) {
            return {
                name: connector.name,
                version: connector.version,
                lib: connector.lib,
                parameters: connector.parameters
            };
        });
        ParameterService.updateConnectors(connectors).then(function() {
            AppDispatcher.dispatch({
                actionName: Constants.events.DISPLAY_MESSAGE,
                data: t('connectorsUpdated')
            });
            self.setState({
                isLoading: false
            });
        }).catch(function() {
            AppDispatcher.dispatch({
                actionName: Constants.events.DISPLAY_MESSAGE,
                data: t('connectorsCannotBeUpdated')
            });
            self.setState({
                isLoading: false
            });
        });
    }

    render() {
        var self = this;
        const { t, classes } = self.props;
        var connectors = [];
        var settingFields = [];
        if (self.state.connectors) {
            self.state.connectors.forEach(function(connector) {
                connectors.push(
                    <Grid item lg={3} md={6} xs={12} sm={12}>
                        <Paper>
                            <div style={{position: 'relative'}}>
                                <div style={{ margin: '15px 30px'}}>
                                    <div>
                                        <img src={connector.picture} width='100' />
                                    </div>
                                    <div>
                                        <Typography variant="title">{connector.name}</Typography>
                                    </div>
                                </div>
                                <div style={{position: 'absolute', top: '40px', right: '80px'}}>
                                    <Switch checked={connector.is_enabled} onClick={ () => self.handleSwitch(connector) } />
                                </div>
                                <div style={{position: 'absolute', top: '5px', right: '5px'}}>
                                    <IconButton onClick={() => self.handleEdit(connector)}><Edit /></IconButton>
                                </div>
                            </div>
                        </Paper>
                    </Grid>
                );
            });
        }

        if (self.state.selectedConnector && self.state.selectedConnector.parameters) {
            for(var parameter in self.state.selectedParameters) {
                settingFields.push(
                    <FormControl fullWidth={true} className={classes.margin}>
                        <InputLabel>{parameter}</InputLabel>
                        <Input value={self.state.selectedParameters[parameter]} name={parameter} onChange={self.handleChangeProperty}  />
                    </FormControl>
                );                
            }
        }

        return (<div className="block">
            <Dialog open={self.state.isModalOpened} onClose={self.handleCloseModal}>
                <DialogTitle>{t('editConnectorSettings')}</DialogTitle>
                {self.state.isSavingConnectorSettings ? (<CircularProgress />) : (
                    <div>
                        <DialogContent>
                            {settingFields}
                        </DialogContent>
                        <DialogActions>
                            <Button  variant="raised" color="primary" onClick={self.handleSaveSettings}>{t('saveSettings')}</Button>
                        </DialogActions>
                    </div>
                )}
            </Dialog>
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
                <div className="header">
                    <div style={{display: 'inline-block'}}>
                        <h4>{t('listOfConnectors')}</h4>
                        <p>{t('openidServerMustBeRestarted')}</p>
                    </div>                    
                    <div style={{float: "right"}}>                        
                        <IconButton onClick={this.handleSave}>
                            <Save />
                        </IconButton>
                    </div>
                </div>
                <div className="body">
                    { self.state.isLoading ? (<CircularProgress />) : (
                        <Grid container spacing={16} style={{ paddingTop: '15px' }}>
                            {connectors}
                        </Grid>
                    )}
                </div>
            </div>
        </div>);
    }

    componentDidMount() {
        this.refreshData();
    }
}

export default translate('common', { wait: process && !process.release })(withStyles(styles)(Connectors));