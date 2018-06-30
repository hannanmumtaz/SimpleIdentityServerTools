import React, { Component } from "react";
import { WebsiteService, SessionService, ScimService } from '../services';
import { withRouter } from 'react-router-dom';
import { withStyles } from 'material-ui/styles';
import { translate } from 'react-i18next';
import { NavLink, Link } from 'react-router-dom';
import { SessionStore } from '../stores';
import { TextField , Button, Grid, IconButton, Checkbox, CircularProgress } from 'material-ui';
import Table, { TableBody, TableCell, TableHead, TableRow, TableFooter, TablePagination, TableSortLabel } from 'material-ui/Table';
import Input, { InputLabel } from 'material-ui/Input';
import { FormControl, FormHelperText } from 'material-ui/Form';
import Save from '@material-ui/icons/Save';
import Visibility from '@material-ui/icons/Visibility';
import $ from 'jquery';
import AppDispatcher from '../appDispatcher';
import Constants from '../constants';


const styles = theme => ({
  margin: {
    margin: theme.spacing.unit,
  }
});

class ViewScimAttribute extends Component {
    constructor(props) {
        super(props);
        this.refresh = this.refresh.bind(this);
        this.handleChangeProperty = this.handleChangeProperty.bind(this);
        this.state = {
            isLoading: true,
            id: null,
            attr: null,
            attribute: {}
        };
    }

    refresh() {
        var self = this;
        self.setState({
            isLoading: true
        });
        const { t } = self.props;
        ScimService.getSchemas().then(function(result) {
            var filteredSchemas = result.filter(function(record) { return record.name === self.state.id; });
            if (!filteredSchemas || filteredSchemas.length !== 1) {                
                AppDispatcher.dispatch({
                    actionName: Constants.events.DISPLAY_MESSAGE,
                    data: t('schemaCannotBeRetrieved')
                });
                self.setState({
                    isLoading: false
                });
                return;
            }

            var schema = filteredSchemas[0];
            var filteredAttribute = schema.attributes.filter(function(attr) { return attr.name === self.state.attr; });
            if (!filteredAttribute || filteredAttribute.length !== 1) {
                AppDispatcher.dispatch({
                    actionName: Constants.events.DISPLAY_MESSAGE,
                    data: t('schemaAttributeCannotBeRetrieved')
                });
                self.setState({
                    isLoading: false
                });
                return;
            }

            var attribute = filteredAttribute[0];
            console.log(attribute);
            self.setState({
                isLoading: false,
                attribute: attribute
            });

        }).catch(function(e) {
            AppDispatcher.dispatch({
                actionName: Constants.events.DISPLAY_MESSAGE,
                data: t('schemaCannotBeRetrieved')
            });
            self.setState({
                isLoading: false
            });
        });
    }

    handleChangeProperty(e) {
        var schema = this.state.schema;
        schema[e.target.name] = e.target.value;
        this.setState({
            schema: schema
        });
    }

    render() {
        var self = this;
        const { t, classes } = self.props;
        var rows = [];
        if (self.state.attribute && self.state.attribute.subAttributes) {
            self.state.attribute.subAttributes.forEach(function(attribute) {
                rows.push(
                    <TableRow>
                        <TableCell>{attribute.name}</TableCell>
                        <TableCell>{attribute.description}</TableCell>
                        <TableCell>{attribute.type}</TableCell>
                        <TableCell>{<Checkbox color="primary" checked={attribute.required} disabled={true} />}</TableCell>
                        <TableCell>{<Checkbox color="primary" checked={attribute.multiValued} disabled={true} />}</TableCell>
                    </TableRow>
                );
            });
        }

        return (<div className="block">
            <div className="block-header">
                <Grid container>
                    <Grid item md={5} sm={12}>
                        <h4>{t('scimSchema')}</h4>
                        <i>{t('scimSchemaDescription')}</i>
                    </Grid>
                    <Grid item md={7} sm={12}>                        
                        <ul className="breadcrumb float-md-right">
                            <li className="breadcrumb-item"><NavLink to="/">{t('websiteTitle')}</NavLink></li>
                            <li className="breadcrumb-item"><NavLink to="/scim/schemas">{t('scimSchemas')}</NavLink></li>
                            <li className="breadcrumb-item"><NavLink to={("/scim/schemas/" + self.state.id)}>{t('scimSchema')}</NavLink></li>
                            <li className="breadcrumb-item">{t('scimSchemaAttribute')}</li>
                        </ul>
                    </Grid>
                </Grid>
            </div>
            <Grid container>
                <Grid item md={12}>
                    <div className="card">
                        <div className="header">
                            <h4 style={{display: "inline-block"}}>{t('scimSchemaInformation')}</h4>
                        </div>
                        <div className="body">
                            { self.state.isLoading ? ( <CircularProgress /> ) : (        
                                <div>
                                    {/* Name */} 
                                    <FormControl fullWidth={true} className={classes.margin} disabled={true}>
                                        <InputLabel>{t('scimSchemaAttributeName')}</InputLabel>
                                        <Input value={self.state.attribute.name} />
                                        <FormHelperText>{t('scimSchemaAttributeNameDescription')}</FormHelperText>
                                    </FormControl>   
                                    {/* Description */} 
                                    <FormControl fullWidth={true} className={classes.margin} disabled={true}>
                                        <InputLabel>{t('scimSchemaAttributeDescription')}</InputLabel>
                                        <Input value={self.state.attribute.description} />
                                        <FormHelperText>{t('scimSchemaAttributeDescriptionDescription')}</FormHelperText>
                                    </FormControl>     
                                    {/* Type */} 
                                    <FormControl fullWidth={true} className={classes.margin} disabled={true}>
                                        <InputLabel>{t('scimSchemaAttributeType')}</InputLabel>
                                        <Input value={self.state.attribute.type} />
                                        <FormHelperText>{t('scimSchemaAttributeTypeDescription')}</FormHelperText>
                                    </FormControl>
                                    {/* Required */} 
                                    <FormControl fullWidth={true} className={classes.margin} disabled={true}>
                                        <Checkbox checked={self.state.attribute.required} />
                                        <FormHelperText>{t('scimSchemaAttributeIsRequiredDescription')}</FormHelperText>
                                    </FormControl>   
                                    {/* MultiValued */} 
                                    <FormControl fullWidth={true} className={classes.margin} disabled={true}>
                                        <Checkbox checked={self.state.attribute.required} />
                                        <FormHelperText>{t('scimSchemaAttributeMultivaluedDescription')}</FormHelperText>
                                    </FormControl>          
                                </div>
                            )}
                        </div>
                    </div>
                </Grid>
                <Grid item md={12}>
                    <div className="card">
                        <div className="header">
                            <h4 style={{display: "inline-block"}}>{t('scimSchemaSubAttributes')}</h4>
                        </div>
                        <div className="body">
                            { self.state.isLoading ? ( <CircularProgress /> ) : (
                                <Table>
                                    <TableHead>
                                        <TableRow>
                                            <TableCell>{t('scimSchemaAttributeName')}</TableCell>
                                            <TableCell>{t('scimSchemaAttributeDescription')}</TableCell>
                                            <TableCell>{t('scimSchemaAttributeType')}</TableCell>
                                            <TableCell>{t('scimSchemaAttributeIsRequired')}</TableCell>
                                            <TableCell>{t('scimSchemaAttributeIsMultiValued')}</TableCell>
                                        </TableRow>
                                    </TableHead>
                                    <TableBody>
                                        {rows}
                                    </TableBody>
                                </Table>
                            )}
                        </div>
                    </div>
                </Grid>
            </Grid>
        </div>);
    }

    componentDidMount() {
        var self = this;
        SessionStore.addChangeListener(function() {
            self.setState({
                id: self.props.match.params.id,
                attr: self.props.match.params.attr
            }, function() {
                self.refresh();
            });
        });
        
        if (SessionStore.getSession().selectedOpenid) {
            self.setState({
                id: self.props.match.params.id,
                attr: self.props.match.params.attr
            }, function() {
                self.refresh();
            });
        }
    }
}

export default translate('common', { wait: process && !process.release })(withRouter(withStyles(styles)(ViewScimAttribute)));