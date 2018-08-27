import React, { Component } from "react";
import { translate } from 'react-i18next';
import { Grid, Checkbox } from 'material-ui';
import { NavLink } from 'react-router-dom';
import { CircularProgress, Button, Select, List, ListItem, ListItemText, MenuItem, IconButton } from 'material-ui';
import { withStyles } from 'material-ui/styles';
import { FormControl, FormHelperText } from 'material-ui/Form';
import Input, { InputLabel } from 'material-ui/Input';
import { ScimService } from '../../services';
import AppDispatcher from '../../appDispatcher';
import Constants from '../../constants';
import { SessionStore } from '../../stores';
import Delete from '@material-ui/icons/Delete';

const styles = theme => ({
    margin: {
        margin: theme.spacing.unit
    }
});

class ScimMappingSettingsTab extends Component {
    constructor(props) {
        super(props);
        this.refreshData = this.refreshData.bind(this);
        this.handleNewSchemaChangeProperty = this.handleNewSchemaChangeProperty.bind(this);
        this.handleAdConfigurationChangeProperty = this.handleAdConfigurationChangeProperty.bind(this);
        this.handleAddSchemaConfiguration = this.handleAddSchemaConfiguration.bind(this);
        this.handleRemoveSchemaConfiguration = this.handleRemoveSchemaConfiguration.bind(this);
        this.handleSaveAdConfiguration = this.handleSaveAdConfiguration.bind(this);
        this.toggleProperty = this.toggleProperty.bind(this);
        this.state = {
            isAdConfigurationLoading: true,
            isScimSchemasLoading: true,
            adConfiguration: {
                schemas: []
            },
            newSchema: {},
            schemas: []
        };
    }

    /**
    * Display the properties
    */
    refreshData() {
        var self = this;
        const { t } = self.props;
        self.setState({
            isAdConfigurationLoading: true,
            isScimSchemasLoading: true
        });
        ScimService.getAdConfiguration().then(function (conf) {
            self.setState({
                adConfiguration: conf,
                isAdConfigurationLoading: false
            });
        }).catch(function () {
            AppDispatcher.dispatch({
                actionName: Constants.events.DISPLAY_MESSAGE,
                data: t('cannotGetAdConfiguration')
            });
            self.setState({
                isAdConfigurationLoading: false,
                adConfiguration: { }
            });
        });
        ScimService.getSchemas().then(function(schemas) {
            var newSchema = self.state.newSchema;
            newSchema['schema_id'] = 'urn:ietf:params:scim:schemas:core:2.0:User';
            self.setState({
                schemas: schemas.map(function(schema) { return { id : schema.id, name: schema.name }; }),
                newSchema: newSchema,
                isScimSchemasLoading: false
            });
        }).catch(function() {
            AppDispatcher.dispatch({
                actionName: Constants.events.DISPLAY_MESSAGE,
                data: t('cannotGetScimSchemas')
            });
            self.setState({
                schemas: [],
                newSchema: {},
                isScimSchemasLoading: false
            });
        });
    }

    /**
     *  Toggle the property
     * */
    toggleProperty() {
        var self = this;
        var adConfiguration = self.state.adConfiguration;
        adConfiguration['is_enabled'] = !adConfiguration['is_enabled'];
        self.setState({
            adConfiguration: adConfiguration
        });
    }

    /**
    * Change the property
    */
    handleNewSchemaChangeProperty(e) {
        var self = this;
        var newSchema = self.state.newSchema;
        newSchema[e.target.name] = e.target.value;
        self.setState({
            newSchema: newSchema
        });
    }

	/**
	* Change the property.
	*/
    handleAdConfigurationChangeProperty(e) {
        var self = this;
        var adConfiguration = self.state.adConfiguration;
        adConfiguration[e.target.name] = e.target.value;
        self.setState({
            adConfiguration: adConfiguration
        });
    }

    /**
    * Add the schema configuration
    */
    handleAddSchemaConfiguration() {
        var self = this;
        var adConfiguration = self.state.adConfiguration;
        if (self.state.newSchema.filter === '' || self.state.newSchema.filter_class === '') {
            return;
        }

        var fileredAdConfig = adConfiguration.schemas.filter(function(ac) { return ac['schema_id'] ===  self.state.newSchema.schema_id; });
        if (fileredAdConfig.length !== 0) {
            return;
        }

        var newSchema = self.state.newSchema;
        adConfiguration.schemas.push({
            schema_id: newSchema['schema_id'],
            filter: newSchema['filter'],
            filter_class: newSchema['filter_class']
        });
        newSchema['filter'] = '';
        newSchema['filter_class'] = ''; 
        self.setState({
            adConfiguration: adConfiguration,
            newSchema: newSchema
        });
    }

    /**
    * Remove the selected schema configuration.
    */
    handleRemoveSchemaConfiguration(id) {
        var self = this;
        var adConfiguration = self.state.adConfiguration;
        var sc = adConfiguration.schemas.filter(function(schema) { return schema.schema_id === id ; })[0];
        var index = adConfiguration.schemas.indexOf(sc);
        adConfiguration.schemas.splice(index, 1);
        self.setState({
            adConfiguration: adConfiguration
        });
    }

    /**
    * Save the configuration
    */
    handleSaveAdConfiguration() {
        var self = this;
        const { t } = self.props;
        self.setState({
            isAdConfigurationLoading: true,
            isScimSchemasLoading: true
        }); 
        ScimService.updateAdConfiguration(self.state.adConfiguration).then(function() {
            self.setState({
                isAdConfigurationLoading: false,
                isScimSchemasLoading: false
            }); 
            AppDispatcher.dispatch({
                actionName: Constants.events.DISPLAY_MESSAGE,
                data: t('adConfigurationUpdated')
            });
        }).catch(function() {
            self.setState({
                isAdConfigurationLoading: false,
                isScimSchemasLoading: false
            }); 
            AppDispatcher.dispatch({
                actionName: Constants.events.DISPLAY_MESSAGE,
                data: t('cannotUpdateAdConfiguration')
            });
        });
    }

    render() {
        var self = this;
        const { t, classes } = self.props;
        var schemas = [];
        var schemaConfigurationItems = [];
        if (self.state.schemas) {
            self.state.schemas.forEach(function(schema) {
                schemas.push(
                    <MenuItem key={schema.id} value={schema.id}>{schema.name}</MenuItem>
                );
            });
        }

        if (self.state.adConfiguration && self.state.adConfiguration.schemas) {
            self.state.adConfiguration.schemas.forEach(function(schema) {
                schemaConfigurationItems.push(
                    <ListItem dense button style={{overflow: 'hidden'}} key={schema.schema_id}>                        
                        <IconButton onClick={() => self.handleRemoveSchemaConfiguration(schema.schema_id)}>
                            <Delete />
                        </IconButton>
                        <ListItemText>{schema.schema_id}, filter : {schema.filter}, filter class: {schema.filter_class}</ListItemText>
                    </ListItem>
                );
            });
        }

        return (
            <div> 
                <Grid container spacing={40}>
                    <Grid item md={6}>
                        {self.state.isAdConfigurationLoading ? (<CircularProgress />) : (
                            <div>
                                {/* Is enabled */}
                                <FormControl fullWidth={true} className={classes.margin}>
                                    <Checkbox color="primary" checked={self.state.adConfiguration.is_enabled} name="is_enabled" onChange={self.toggleProperty} />
                                    <FormHelperText>{t('isScimMappingEnabledDescription')}</FormHelperText>
                                </FormControl>
                                {/* IP ADDR*/ }
                                <FormControl fullWidth={true} className={classes.margin}>
                                    <InputLabel>{t('adIpAddr')}</InputLabel>
                                    <Input value={self.state.adConfiguration.ip_adr} name="ip_adr" onChange={self.handleAdConfigurationChangeProperty} />
                                    <FormHelperText>{t('adIpAddrDescription')}</FormHelperText>
                                </FormControl>
                                {/* Port */}
                                <FormControl fullWidth={true} className={classes.margin}>
                                    <InputLabel>{t('adPort')}</InputLabel>
                                    <Input type="number" value={self.state.adConfiguration.port} name="port" onChange={self.handleAdConfigurationChangeProperty} />
                                    <FormHelperText>{t('adPortDescription')}</FormHelperText>
                                </FormControl>
                                {/* Username */}
                                <FormControl fullWidth={true} className={classes.margin}>
                                    <InputLabel>{t('adUserName')}</InputLabel>
                                    <Input value={self.state.adConfiguration.username} name="username" onChange={self.handleAdConfigurationChangeProperty} />
                                    <FormHelperText>{t('adUserNameDescription')}</FormHelperText>
                                </FormControl>
                                {/* Password */}
                                <FormControl fullWidth={true} className={classes.margin}>
                                    <InputLabel>{t('adPassword')}</InputLabel>
                                    <Input value={self.state.adConfiguration.password} name="password" onChange={self.handleAdConfigurationChangeProperty} />
                                    <FormHelperText>{t('adPasswordDescription')}</FormHelperText>
                                </FormControl>
                                {/* DistinguishedName */}
                                <FormControl fullWidth={true} className={classes.margin}>
                                    <InputLabel>{t('adDistinguishedName')}</InputLabel>
                                    <Input value={self.state.adConfiguration.distinguished_name} name="distinguished_name" onChange={self.handleAdConfigurationChangeProperty} />
                                    <FormHelperText>{t('adDistinguishedNameDescription')}</FormHelperText>
                                </FormControl>
                                <Button variant="raised" color="primary" onClick={self.handleSaveAdConfiguration}>{t('saveAdConfiguration')}</Button>
                            </div>
                        )}
                    </Grid>
                    <Grid item md={6}> 
                        { !self.state.isScimSchemasLoading && (   
                            <div>                             
                                {/* Select SCHEMA identifier */}
                                <FormControl fullWidth={true} className={classes.margin}>
                                    <InputLabel>{t('schemaId')}</InputLabel>
                                    <Select value={self.state.newSchema.schema_id} name='schema_id' onChange={self.handleNewSchemaChangeProperty}>
                                        {schemas}
                                    </Select>
                                    <FormHelperText>{t('schemaIdDescription')}</FormHelperText>
                                </FormControl>
                                {/* Filter */}
                                <FormControl fullWidth={true} className={classes.margin}>
                                    <InputLabel>{t('adFilter')}</InputLabel>
                                    <Input type='text' name='filter' value={self.state.newSchema.filter} onChange={self.handleNewSchemaChangeProperty} />
                                    <FormHelperText>{t('adFilterDescription')}</FormHelperText>
                                </FormControl>
                                {/* Filter class */}
                                <FormControl fullWidth={true} className={classes.margin}>
                                    <InputLabel>{t('adFilterClass')}</InputLabel>
                                    <Input type='text' name='filter_class' value={self.state.newSchema.filter_class} onChange={self.handleNewSchemaChangeProperty} />
                                    <FormHelperText>{t('adFilterClassDescription')}</FormHelperText>
                                </FormControl>
                                <Button variant="raised" color="primary" onClick={self.handleAddSchemaConfiguration}>{t('addSchemaConfiguration')}</Button>
                            </div>
                        )}
                        { !self.state.isAdConfigurationLoading &&(
                            <List>
                                {schemaConfigurationItems}
                            </List>
                        )}
                    </Grid>
                </Grid>
            </div>
        );
    }

    componentDidMount() {
        var self = this;
        SessionStore.addChangeListener(function () {
            self.refreshData();
        });

        if (SessionStore.getSession().selectedOpenid) {
            self.refreshData();
        }        
    }
}

export default translate('common', { wait: process && !process.release })(withStyles(styles)(ScimMappingSettingsTab));