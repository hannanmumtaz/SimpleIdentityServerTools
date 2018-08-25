import React, { Component } from "react";
import { translate } from 'react-i18next';
import { NavLink } from 'react-router-dom';
import { ScimService } from '../services';
import { withStyles } from 'material-ui/styles';
import { CircularProgress, Checkbox, Grid, IconButton, Menu, MenuItem, InputLabel, Input, Select } from 'material-ui';
import Table, { TableBody, TableCell, TableHead, TableRow, TableFooter } from 'material-ui/Table';
import Dialog, { DialogTitle, DialogContent, DialogActions } from 'material-ui/Dialog';
import { FormControl, FormHelperText } from 'material-ui/Form';
import Visibility from '@material-ui/icons/Visibility';
import MoreVert from '@material-ui/icons/MoreVert';
import AppDispatcher from '../appDispatcher';
import Constants from '../constants';
import { SessionStore } from '../stores';

const styles = theme => ({
    margin: {
        margin: theme.spacing.unit,
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
        this.refreshData = this.refreshData.bind(this);
        this.refreshSchema = this.refreshSchema.bind(this);
        this.state = {
            isLoading: true,
            anchorEl: null,
            isModalOpened: false,
            schemas: [],
            selectedSchema: '',
            scimAttributes: [],
            selectedScimAttribute: '',
            adProperties: [],
            selectedAdProperty: ''
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
            selectedSchema: value
        }, function () {
            self.refreshSchema();
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
        ScimService.getSchemas().then(function (schemas) {
            self.setState({
                schemas: schemas.map(function (s) { return { id: s.id, name: s.name }; }),
                selectedSchema: 'urn:ietf:params:scim:schemas:core:2.0:User'
            }, function () {
                self.refreshSchema();
            });
        }).catch(function () {
            AppDispatcher.dispatch({
                actionName: Constants.events.DISPLAY_MESSAGE,
                data: t('cannotRetrieveScimSchemas')
            });
            self.setState({
                schemas: [],
                selectedSchema: ''
            });
        });
        ScimService.getAllMappings().then(function (mappings) {
            self.setState({
                isLoading: false
            });
        }).catch(function (e) {
            AppDispatcher.dispatch({
                actionName: Constants.events.DISPLAY_MESSAGE,
                data: t('cannotRetrieveScimMappings')
            });
            self.setState({
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
        ScimService.getSchema(self.state.selectedSchema).then(function (currentSchema) {
            self.setState({
                scimAttributes: currentSchema.attributes.map(function (a) { return { id: a.id, name: a.name }; }),
                selectedScimAttribute: currentSchema.attributes[0].id
            });
        }).catch(function () {
            AppDispatcher.dispatch({
                actionName: Constants.events.DISPLAY_MESSAGE,
                data: t('cannotGetSchemaInformation')
            });
            self.setState({
                scimAttributes: [],
                selectedScimAttribute: ''
            });
        });
        ScimService.getAllAdProperties(self.state.selectedSchema).then(function (adProperties) {
            self.setState({
                adProperties: adProperties,
                selectedAdProperty: adProperties[0]
            });
        }).catch(function () {
            self.setState({
                adProperties: [],
                selectedAdProperty: ''
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
        if (self.state.schemas) {
            self.state.schemas.forEach(function (schema) {
                schemaOpts.push(
                    <MenuItem id={schema.id} value={schema.id}>{schema.name}</MenuItem>
                );
            });
        }

        return (<div className="block">
            <Dialog open={self.state.isModalOpened} onClose={this.handleCloseModal}>
                <DialogTitle>{t('addAdMapping')}</DialogTitle>
                <div>
                    <DialogContent>
                        {/* Select schema */ }
                        <FormControl fullWidth={true} className={classes.margin}>
                            <InputLabel>{t('schemaType')}</InputLabel>    
                            <Select value={self.state.selectedSchema} onChange={self.handleChangeSchemaId}>
                                {schemaOpts}
                            </Select>
                            <FormHelperText>{t('schemaTypeDescription')}</FormHelperText>
                        </FormControl>
                    </DialogContent>
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
                                    <TableCell><Checkbox color="primary" /></TableCell>
                                    <TableCell>{t('propertyType')}</TableCell>
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