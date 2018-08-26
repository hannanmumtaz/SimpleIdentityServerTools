import React, { Component } from "react";
import { translate } from 'react-i18next';
import { NavLink } from 'react-router-dom';
import { ScimService } from '../services';
import { withStyles } from 'material-ui/styles';
import { CircularProgress, Checkbox, Grid, IconButton, Button, Menu, MenuItem, InputLabel, Input, Select } from 'material-ui';
import Table, { TableBody, TableCell, TableHead, TableRow, TableFooter } from 'material-ui/Table';
import Dialog, { DialogTitle, DialogContent, DialogActions } from 'material-ui/Dialog';
import { FormControl, FormHelperText } from 'material-ui/Form';
import Visibility from '@material-ui/icons/Visibility';
import Delete from '@material-ui/icons/Delete';
import MoreVert from '@material-ui/icons/MoreVert';
import AppDispatcher from '../appDispatcher';
import Constants from '../constants';
import { SessionStore } from '../stores';

const styles = theme => ({
    margin: {
        margin: theme.spacing.unit
    }
});

class ScimMappings extends Component {
    constructor(props) {
        super(props);
        this.handleClick = this.handleClick.bind(this);
        this.handleClose = this.handleClose.bind(this);
        this.handleOpenModal = this.handleOpenModal.bind(this);
        this.handleCloseModal = this.handleCloseModal.bind(this);
        this.handleChangeSchemaId = this.handleChangeSchemaId.bind(this);
        this.handleChangeProperty = this.handleChangeProperty.bind(this);
        this.handleAddScimMapping = this.handleAddScimMapping.bind(this);
        this.handleSelectScimAttribute = this.handleSelectScimAttribute.bind(this);
        this.handleAllSelections = this.handleAllSelections.bind(this);
        this.handleRemoveClients = this.handleRemoveClients.bind(this);
        this.handleRowClick = this.handleRowClick.bind(this);
        this.refreshData = this.refreshData.bind(this);
        this.refreshSchema = this.refreshSchema.bind(this);
        this.state = {
            isLoading: true,
            isScimAttributesLoading: false,
            isAdPropertiesLoading: false,
            isAddingScimMapping: false,
            anchorEl: null,
            isModalOpened: false,
            schemas: [],
            selectedSchema: '',
            scimAttributes: [],
            selectedScimAttribute: '',
            adProperties: [],
            selectedAdProperty: '',
            subScimAttributes: [],
            selectedSubScimAttribute: '',
            mappings: []
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
    * Open the modal.
    */
    handleOpenModal() {
        this.setState({
            isModalOpened: true
        });
    }

    /**
    * Close the modal.
    */
    handleCloseModal() {
        this.setState({
            isModalOpened: false
        });
    }

    /**
    * Change the schema id.
    */
    handleChangeSchemaId(e) {
        var self = this;
        var value = e.target.value;
        self.setState({
            selectedSchema: value,
            selectedAdProperty: '',
            selectedSubScimAttribute: '',
            selectedScimAttribute: '',
            scimAttributes: [],
            subScimAttributes: [],
            adProperties: [],

        }, function () {
            self.refreshSchema();
        });
    }

	/**
	* Change the property.
	*/
    handleChangeProperty(e) {
        var self = this;
        self.setState({
            [e.target.name]: e.target.value
        });
    }

    /**
    * Add the SCIM mapping.
    */
    handleAddScimMapping() {
        var self = this;
        const { t } = self.props;
        self.setState({
            isAddingScimMapping: true
        });
        var request = {
            attribute_id: self.state.selectedSubScimAttribute === '' ? self.state.selectedScimAttribute : self.state.selectedSubScimAttribute,
            ad_property_name: self.state.selectedAdProperty,
            schema_id: self.state.selectedSchema
        };
        ScimService.addScimMapping(request).then(function () {
            AppDispatcher.dispatch({
                actionName: Constants.events.DISPLAY_MESSAGE,
                data: t('scimMappingAdded')
            });
            self.setState({
                isAddingScimMapping: false,
                isModalOpened: false
            });
            self.refreshData();
        }).catch(function () {
            AppDispatcher.dispatch({
                actionName: Constants.events.DISPLAY_MESSAGE,
                data: t('scimMappingCannotBeAdded')
            });
            self.setState({
                isAddingScimMapping: false
            });
        });
    }

    /**
    * Select the SCIM attribute
    */
    handleSelectScimAttribute(e) {
        var self = this;
        var scimAttribute = self.state.scimAttributes.filter(function (sa) { return sa.id === e.target.value; })[0];
        var subScimAttributes = [];
        var selectedSubScimAttribute = '';
        if (scimAttribute.type === 'complex') {
            subScimAttributes = scimAttribute.subAttributes.map(function (sa) { return { id: sa.id, name: sa.name }; });
            selectedSubScimAttribute = subScimAttributes[0].id;
        }

        self.setState({
            selectedScimAttribute: e.target.value,
            subScimAttributes: subScimAttributes,
            selectedSubScimAttribute: selectedSubScimAttribute
        });
    }

    /**
    * Handle click on the row.
    */
    handleRowClick(e, record) {
        record.isSelected = e.target.checked;
        var data = this.state.mappings;
        var nbSelectedRecords = data.filter(function (r) { return r.isSelected; }).length;
        this.setState({
            mappings: data,
            isRemoveDisplayed: nbSelectedRecords > 0
        });
    }

    /**
    * Select / Unselect all mappings.
    */
    handleAllSelections(e) {
        var checked = e.target.checked;
        var mappings = this.state.mappings;
        mappings.forEach(function (r) { r.isSelected = checked; });
        this.setState({
            mappings: mappings,
            isRemoveDisplayed: checked
        });
    }

    /**
    * Remove the selected clients 
    */
    handleRemoveClients() {
        var self = this;
        const { t } = self.props;
        self.setState({
            isLoading: true
        });
        var operations = [];
        self.state.mappings.filter(function (m) { return m.isSelected; }).forEach(function (m) {
            operations.push(ScimService.removeScimMapping(m['attributeId']));
        });
        Promise.all(operations).then(function () {
            AppDispatcher.dispatch({
                actionName: Constants.events.DISPLAY_MESSAGE,
                data: t('removeScimMappings')
            });
            self.setState({
                isRemoveDisplayed: false
            });
            self.refreshData();
        }).catch(function () {
            self.setState({
                isLoading: false
            });
            AppDispatcher.dispatch({
                actionName: Constants.events.DISPLAY_MESSAGE,
                data: t('cannotRemoveScimMappings')
            });
        });
    }

    /**
     *  Display the data.
     */
    refreshData() {
        var self = this;
        const { t } = self.props;
        self.setState({
            isLoading: true
        });
        ScimService.getSchemas().then(function (schemas) { // Get all SCIM schemas.
            self.setState({
                schemas: schemas.map(function (s) { return { id: s.id, name: s.name }; }),
                selectedSchema: 'urn:ietf:params:scim:schemas:core:2.0:User'
            }, function () {
                self.refreshSchema();
            });
            ScimService.getAllMappings().then(function (mappingsResult) { // Get all the mappings.
                var mappings = [];
                mappingsResult.forEach(function (mapping) {
                    var schema = schemas.filter(function (s) { return s.id === mapping['schema_id']; })[0];
                    var attributeName = '';
                    schema.attributes.forEach(function (attr) {
                        if (attr.id === mapping['attribute_id']) {
                            attributeName = attr.name;
                            return;
                        }

                        if (attr.subAttributes) {
                            attr.subAttributes.forEach(function (attr) {
                                if (attr.id === mapping['attribute_id']) {
                                    attributeName = attr.name;
                                    return;
                                }
                            });
                        }
                    });

                    mappings.push({
                        attributeName: attributeName,
                        adPropertyName: mapping['ad_property_name'],
                        attributeId: mapping['attribute_id'],
                        schemaName: schema.name,
                        isSelected: false
                    });
                });
                self.setState({
                    isLoading: false,
                    mappings: mappings
                });
            }).catch(function (e) {
                AppDispatcher.dispatch({
                    actionName: Constants.events.DISPLAY_MESSAGE,
                    data: t('cannotRetrieveScimMappings')
                });
                self.setState({
                    isLoading: false,
                    mappings: []
                });
            });
        }).catch(function () {
            AppDispatcher.dispatch({
                actionName: Constants.events.DISPLAY_MESSAGE,
                data: t('cannotRetrieveScimSchemas')
            });
            self.setState({
                schemas: [],
                selectedSchema: '',
                isLoading: false
            });
        });
    }

    /**
    * Display information of the current schema.
    */
    refreshSchema() {
        var self = this;
        const { t } = self.props;
        self.setState({
            isScimAttributesLoading: true,
            isAdPropertiesLoading: true
        });
        ScimService.getSchema(self.state.selectedSchema).then(function (currentSchema) {
            self.setState({
                scimAttributes: currentSchema.attributes.map(function (a) { return { id: a.id, name: a.name, subAttributes: a.subAttributes, type: a.type }; }),
                selectedScimAttribute: currentSchema.attributes[0].id,
                isScimAttributesLoading: false
            });
        }).catch(function () {
            AppDispatcher.dispatch({
                actionName: Constants.events.DISPLAY_MESSAGE,
                data: t('cannotGetSchemaInformation')
            });
            self.setState({
                scimAttributes: [],
                selectedScimAttribute: '',
                isScimAttributesLoading: false
            });
        });
        ScimService.getAllAdProperties(self.state.selectedSchema).then(function (adProperties) {
            self.setState({
                adProperties: adProperties,
                selectedAdProperty: adProperties[0],
                isAdPropertiesLoading: false
            });
        }).catch(function () {
            self.setState({
                adProperties: [],
                selectedAdProperty: '',
                isAdPropertiesLoading: false
            });
            AppDispatcher.dispatch({
                actionName: Constants.events.DISPLAY_MESSAGE,
                data: t('cannotRetrieveAdProperties')
            });
        });
    }

    render() {
        var self = this;
        const { t, classes } = self.props;
        var rows = [];
        var schemaOpts = [];
        var scimAttributes = [];
        var subScimAttributes = [];
        var adProperties = [];
        if (self.state.schemas) {
            self.state.schemas.forEach(function (schema) {
                schemaOpts.push(
                    <MenuItem id={schema.id} value={schema.id}>{schema.name}</MenuItem>
                );
            });
        }

        if (self.state.scimAttributes) {
            self.state.scimAttributes.forEach(function (attr) {
                scimAttributes.push(
                    <MenuItem id={attr.id} value={attr.id}>{attr.name}</MenuItem>
                );
            });
        }

        if (self.state.subScimAttributes) {
            self.state.subScimAttributes.forEach(function (attr) {
                subScimAttributes.push(
                    <MenuItem id={attr.id} value={attr.id}>{attr.name}</MenuItem>
                );
            });
        }

        if (self.state.adProperties) {
            self.state.adProperties.forEach(function (adProperty) {
                adProperties.push(
                    <MenuItem id={adProperty} value={adProperty}>{adProperty}</MenuItem>
                );
            });
        }

        if (self.state.mappings) {
            self.state.mappings.forEach(function (m) {
                rows.push(
                    <TableRow>
                        <TableCell><Checkbox color="primary" checked={m.isSelected} onChange={(e) => self.handleRowClick(e, m)} /></TableCell>
                        <TableCell>{m.schemaName}</TableCell>
                        <TableCell>{m.attributeName}</TableCell>
                        <TableCell>{m.adPropertyName}</TableCell>
                        <TableCell></TableCell>
                     </TableRow>
                );
            });
        }

        return (<div className="block">
            <Dialog open={self.state.isModalOpened} onClose={this.handleCloseModal}>
                <DialogTitle>{t('addAdMapping')}</DialogTitle>
                <div>
                    <DialogContent>
                        {self.state.isAddingScimMapping ? (<CircularProgress />) : (
                            <div>
                                {/* Select schema */}
                                <FormControl fullWidth={true} className={classes.margin}>
                                    <InputLabel>{t('schemaType')}</InputLabel>
                                    <Select value={self.state.selectedSchema} onChange={self.handleChangeSchemaId}>
                                        {schemaOpts}
                                    </Select>
                                    <FormHelperText>{t('schemaTypeDescription')}</FormHelperText>
                                </FormControl>
                                {/* Select property name */}
                                {self.state.isScimAttributesLoading ? (<CircularProgress />) : (
                                    <Grid container>
                                        <Grid item md={6}>
                                            <FormControl className={classes.margin}>
                                                <InputLabel>{t('scimAttribute')}</InputLabel>
                                                <Select value={self.state.selectedScimAttribute} name="selectedScimAttribute" onChange={self.handleSelectScimAttribute}>
                                                    {scimAttributes}
                                                </Select>
                                                <FormHelperText>{t('scimAttributeDescription')}</FormHelperText>
                                            </FormControl>
                                        </Grid>
                                        <Grid item md={6}>
                                            {subScimAttributes.length === 0 ? (<span>{t('noSubScimAttributes')}</span>) : (
                                                <FormControl className={classes.margin}>
                                                    <InputLabel>{t('subScimAttribute')}</InputLabel>
                                                    <Select value={self.state.selectedSubScimAttribute} name="selectedSubScimAttribute" onChange={self.handleChangeProperty}>
                                                        {subScimAttributes}
                                                    </Select>
                                                    <FormHelperText>{t('subScimAttributeDescription')}</FormHelperText>
                                                </FormControl>
                                            )}
                                        </Grid>
                                    </Grid>
                                )}
                                {/* Select AD property name */}
                                {self.state.isAdPropertiesLoading ? (<CircularProgress />) : (
                                    <FormControl fullWidth={true} className={classes.margin}>
                                        <InputLabel>{t('adPropertyName')}</InputLabel>
                                        <Select value={self.state.selectedAdProperty} name="selectedAdProperty" onChange={self.handleChangeProperty}>
                                            {adProperties}
                                        </Select>
                                        <FormHelperText>{t('adPropertyNameDescription')}</FormHelperText>
                                    </FormControl>
                                )}
                            </div>
                        )}
                    </DialogContent>
                    <DialogActions>
                        <Button variant="raised" color="primary" onClick={self.handleAddScimMapping}>{t('addScimMapping')}</Button>
                    </DialogActions>
                </div>
            </Dialog>
            <div className="block-header">
                <Grid container>
                    <Grid item md={5} sm={12}>
                        <h4>{t('scimMappingsTitle')}</h4>
                        <i>{t('scimMappingsShortDescription')}</i>
                    </Grid>
                    <Grid item md={7} sm={12}>                        
                        <ul className="breadcrumb float-md-right">
                            <li className="breadcrumb-item"><NavLink to="/">{t('websiteTitle')}</NavLink></li>
                            <li className="breadcrumb-item">{t('scimMappingsTitle')}</li>
                        </ul>
                    </Grid>
                </Grid>
            </div>
            <div className="card">
                <div className="header">
                    <h4 style={{ display: "inline-block" }}>{t('scimMappings')}</h4>
                    <div style={{ float: "right" }}>
                        {self.state.isRemoveDisplayed && (
                            <IconButton onClick={self.handleRemoveClients}>
                                <Delete />
                            </IconButton>
                        )}
                        <IconButton onClick={this.handleClick}>
                            <MoreVert />
                        </IconButton>
                        <Menu anchorEl={self.state.anchorEl} open={Boolean(self.state.anchorEl)} onClose={self.handleClose}>
                            <MenuItem onClick={self.handleOpenModal}>{t('addAdMapping')}</MenuItem>
                        </Menu>
                    </div>
                </div>
                <div className="body">
                    {self.state.isLoading ? (<CircularProgress />) : (
                        <Table>
                            <TableHead>
                                <TableRow>
                                    <TableCell><Checkbox color="primary" onChange={self.handleAllSelections} /></TableCell>
                                    <TableCell>{t('schemaName')}</TableCell>
                                    <TableCell>{t('scimProperty')}</TableCell>
                                    <TableCell>{t('adPropertyName')}</TableCell>
                                    <TableCell></TableCell>
                                </TableRow>
                            </TableHead>
                            <TableBody>
                                {rows}
                            </TableBody>
                        </Table>
                    )}
                </div>
            </div>
        </div>);
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

export default translate('common', { wait: process && !process.release })(withStyles(styles)(ScimMappings));