import React, { Component } from "react";
import { translate } from 'react-i18next';
import { Grid } from 'material-ui';
import { NavLink } from 'react-router-dom';
import { Checkbox, CircularProgress, IconButton } from 'material-ui';
import { OfficeDocumentService } from '../services';
import Table, { TableBody, TableCell, TableHead, TableRow, TableFooter, TablePagination, TableSortLabel } from 'material-ui/Table';
import Visibility from '@material-ui/icons/Visibility'; 
import AppDispatcher from '../appDispatcher';
import Constants from '../constants';

class OfficeDocuments extends Component {
    constructor(props) {
        super(props);
        this.refreshData = this.refreshData.bind(this);
        this.handleChangePage = this.handleChangePage.bind(this);
        this.handleChangeRowsPage = this.handleChangeRowsPage.bind(this);
        this.state = {
            isLoading: false,
            page: 0,
            pageSize: 5,
            count: 0
        };
    }

    refreshData() {
        var self = this;
        self.setState({
            isLoading: true
        });        
        var startIndex = self.state.page * self.state.pageSize;
        var request = { start_index: startIndex, count: self.state.pageSize };
        OfficeDocumentService.search(request).then(function(r) {
            self.setState({
                isLoading: false,
                count: r.total_results,
                officeDocuments: r.content
            });        
        }).catch(function() {
            self.setState({
                isLoading: false,
                officeDocuments: []
            });        
            AppDispatcher.dispatch({
                actionName: Constants.events.DISPLAY_MESSAGE,
                data: t('officeDocumentsCannotBeRetrieved')
            });
        });
    }

    /**
    * Execute when the page has changed.
    */
    handleChangePage(evt, page) {
        var self = this;
        this.setState({
            page: page
        }, () => {
            self.refreshData();
        });
    }

    /**
    * Execute when the number of records has changed.
    */
    handleChangeRowsPage(evt) {
        var self = this;
        self.setState({
            pageSize: evt.target.value
        }, () => {
            self.refreshData();
        });
    }

    render() {
        var self = this;
        const { t } = self.props;
        var rows = [];
        if(self.state.officeDocuments) {
            self.state.officeDocuments.forEach(function(officeDocument) {
                rows.push(
                    <TableRow>
                        <TableCell><Checkbox variant="primary" /></TableCell>
                        <TableCell>{officeDocument.display_name}</TableCell>
                        <TableCell>
                            <NavLink to={'/documents/' + officeDocument.id}>
                                <IconButton>
                                    <Visibility />
                                </IconButton>
                            </NavLink>
                        </TableCell>
                    </TableRow>
                );
            });
        }
        
        return (<div className="block">
            <div className="block-header">
                <Grid container>
                    <Grid item md={5} sm={12}>
                        <h4>{t('officeDocumentsTitle')}</h4>
                        <i>{t('officeDocumentsShortDescription')}</i>
                    </Grid>
                    <Grid item md={7} sm={12}>                        
                        <ul className="breadcrumb float-md-right">
                            <li className="breadcrumb-item"><NavLink to="/">{t('websiteTitle')}</NavLink></li>
                            <li className="breadcrumb-item">{t('officeDocumentsTitle')}</li>
                        </ul>
                    </Grid>
                </Grid>
            </div>
            <div className="card">
                <div className="header">
                    <h4 style={{display: "inline-block"}}>{t('listOfOfficeDocuments')}</h4>
                </div>
                <div className="body">
                    { self.state.isLoading ? (<CircularProgress />) : (
                        <div>
                            <Table>
                                <TableHead>
                                    <TableRow>
                                        <TableCell><Checkbox color="primary" /></TableCell>
                                        <TableCell>{t('documentName')}</TableCell>
                                        <TableCell></TableCell>
                                    </TableRow>
                                </TableHead>
                                <TableBody>
                                    {rows}
                                </TableBody>
                            </Table>
                            <TablePagination component="div" count={self.state.count} rowsPerPage={self.state.pageSize} page={this.state.page} onChangePage={self.handleChangePage} onChangeRowsPerPage={self.handleChangeRowsPage} />
                        </div>
                    )}
                </div>
            </div>
        </div>);
    }

    componentDidMount() {
        var self = this;
        self.refreshData();
    }
}

export default translate('common', { wait: process && !process.release })(OfficeDocuments);