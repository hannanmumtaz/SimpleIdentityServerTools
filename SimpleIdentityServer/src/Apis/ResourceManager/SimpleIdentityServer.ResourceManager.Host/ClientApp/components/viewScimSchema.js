import React, { Component } from "react";
import { WebsiteService, SessionService } from '../services';
import { withRouter } from 'react-router-dom';
import { withStyles } from 'material-ui/styles';
import { translate } from 'react-i18next';
import { NavLink, Link } from 'react-router-dom';
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

class ViewScimSchema extends Component {
    constructor(props) {
        super(props);
        this.refresh = this.refresh.bind(this);
        this.handleChangeProperty = this.handleChangeProperty.bind(this);
        this.state = {
            isLoading: true,
            id: null,
            schema: {}
        };
    }

    refresh() {
        var self = this;
        self.setState({
            isLoading: true
        });
        const { t } = self.props;
        $.get('http://localhost:60001/Schemas').then(function(result) {
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
            self.setState({
                isLoading: false,
                schema: schema
            });

        }).catch(function() {
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
        if (self.state.schema && self.state.schema.attributes) {
            self.state.schema.attributes.forEach(function(attribute) {
                rows.push(
                    <TableRow>
                        <TableCell>{attribute.name}</TableCell>
                        <TableCell>{attribute.type}</TableCell>
                        <TableCell>{<Checkbox color="primary" checked={attribute.required} disabled={true} />}</TableCell>
                        <TableCell>{<Checkbox color="primary" checked={attribute.multiValued} disabled={true} />}</TableCell>
                        <TableCell>
                            <IconButton onClick={ () => self.props.history.push('/scimSchemas/' + self.state.id + '/' + attribute.name) }><Visibility /></IconButton>
                        </TableCell>
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
                            <li className="breadcrumb-item"><NavLink to="/scimSchemas">{t('scimSchemas')}</NavLink></li>
                            <li className="breadcrumb-item">{t('scimSchema')}</li>
                        </ul>
                    </Grid>
                </Grid>
            </div>
            <Grid container spacing={40}>
                <Grid item md={12}>
                    <div className="card">
                        <div className="header">
                            <h4 style={{display: "inline-block"}}>{t('scimSchemaInformation')}</h4>
                        </div>
                        <div className="body">
                            { self.state.isLoading ? ( <CircularProgress /> ) : (        
                                <div>
                                    {/* Id */}
                                    <FormControl fullWidth={true} className={classes.margin} disabled={true}>
                                        <InputLabel>{t('scimSchemaId')}</InputLabel>
                                        <Input value={self.state.schema.id} />
                                        <FormHelperText>{t('scimSchemaIdDescription')}</FormHelperText>
                                    </FormControl>
                                    {/* Name */}
                                    <FormControl fullWidth={true} className={classes.margin}>
                                        <InputLabel>{t('scimSchemaName')}</InputLabel>
                                        <Input value={self.state.schema.name} name="name" onChange={self.handleChangeProperty}  />
                                        <FormHelperText>{t('scimSchemaNameDescription')}</FormHelperText>
                                    </FormControl>    
                                    {/* Description */}
                                  <FormControl fullWidth={true} className={classes.margin}>
                                        <InputLabel>{t('scimSchemaDescription')}</InputLabel>
                                        <Input value={self.state.schema.description} name="description" onChange={self.handleChangeProperty}  />
                                        <FormHelperText>{t('scimSchemaDescriptionDescription')}</FormHelperText>
                                    </FormControl>                       
                                </div>
                            )}
                        </div>
                    </div>
                </Grid>
                <Grid item md={12}>
                    <div className="card">
                        <div className="header">
                            <h4 style={{display: "inline-block"}}>{t('scimSchemaAttributes')}</h4>
                        </div>
                        <div className="body">
                            { self.state.isLoading ? ( <CircularProgress /> ) : (
                                <Table>
                                    <TableHead>
                                        <TableRow>
                                            <TableCell>{t('scimSchemaAttributeName')}</TableCell>
                                            <TableCell>{t('scimSchemaAttributeType')}</TableCell>
                                            <TableCell>{t('scimSchemaAttributeIsRequired')}</TableCell>
                                            <TableCell>{t('scimSchemaAttributeIsMultiValued')}</TableCell>
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
                </Grid>
            </Grid>
        </div>);
    }

    componentDidMount() {
        var self = this;
        self.setState({
            id: self.props.match.params.id
        }, function() {
            self.refresh();
        });
    }
}

export default translate('common', { wait: process && !process.release })(withRouter(withStyles(styles)(ViewScimSchema)));