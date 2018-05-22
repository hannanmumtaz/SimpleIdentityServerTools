import React, { Component } from "react";
import { WebsiteService, SessionService, ScimService } from '../services';
import { withRouter } from 'react-router-dom';
import { translate } from 'react-i18next';
import { NavLink, Link } from 'react-router-dom';
import { TextField , Button, Grid, IconButton, CircularProgress } from 'material-ui';
import Table, { TableBody, TableCell, TableHead, TableRow, TableFooter, TablePagination, TableSortLabel } from 'material-ui/Table';
import Visibility from '@material-ui/icons/Visibility';
import $ from 'jquery';
import AppDispatcher from '../appDispatcher';
import Constants from '../constants';


class ScimSchemas extends Component {
    constructor(props) {
        super(props);
        this.refresh = this.refresh.bind(this);
        this.state = {
            isLoading: false,
            schemas: []
        };
    }

    refresh() {
        var self = this;
        const {t} = self.props;
        self.setState({
            isLoading: true
        });
        ScimService.getSchemas().then(function(result) {
            var schemas = [];
            if (result) {
                result.forEach(function(record) {
                    schemas.push({
                        id: record.id,
                        name: record.name,
                        nbAttributes: record.attributes.length
                    });
                });
            }

            self.setState({
                isLoading: false,
                schemas: schemas
            });
        }).catch(function() {
            AppDispatcher.dispatch({
                actionName: Constants.events.DISPLAY_MESSAGE,
                data: t('schemasCannotBeRetrieved')
            });
            self.setState({
                isLoading: false
            });
        });
    }

    render() {
        var self = this;
        const { t } = self.props;
        var rows = [];
        if (self.state.schemas) {
            self.state.schemas.forEach(function(schema) {
                rows.push(
                    <TableRow>
                        <TableCell>{schema.id}</TableCell>
                        <TableCell>{schema.name}</TableCell>
                        <TableCell>{schema.nbAttributes}</TableCell>
                        <TableCell>
                            <IconButton onClick={ () => self.props.history.push('/scimSchemas/' + schema.name) }><Visibility /></IconButton>
                        </TableCell>
                    </TableRow>
                );
            });
        }

        return (<div className="block">
            <div className="block-header">
                <Grid container>
                    <Grid item md={5} sm={12}>
                        <h4>{t('scimSchemas')}</h4>
                        <i>{t('scimSchemasDescription')}</i>
                    </Grid>
                    <Grid item md={7} sm={12}>                        
                        <ul className="breadcrumb float-md-right">
                            <li className="breadcrumb-item"><NavLink to="/">{t('websiteTitle')}</NavLink></li>
                            <li className="breadcrumb-item">{t('scimSchemas')}</li>
                        </ul>
                    </Grid>
                </Grid>
            </div>
            <div className="card">
                <div className="header">
                    <h4 style={{display: "inline-block"}}>{t('scimSchemas')}</h4>
                </div>
                <div className="body">
                    { this.state.isLoading ? (<CircularProgress />) : (
                        <div>
                            <Table>
                                <TableHead>
                                    <TableCell>{t('scimSchemaId')}</TableCell>
                                    <TableCell>{t('scimSchemaName')}</TableCell>
                                    <TableCell>{t('scimSchemaNbAttributes')}</TableCell>
                                    <TableCell></TableCell>
                                </TableHead>
                                <TableBody>
                                    {rows}
                                </TableBody>
                            </Table>
                        </div>
                    )}
                </div>
            </div>
        </div>);
    }

    componentDidMount() {
        var self = this;
        self.refresh();
    }
}

export default translate('common', { wait: process && !process.release })(withRouter(ScimSchemas));