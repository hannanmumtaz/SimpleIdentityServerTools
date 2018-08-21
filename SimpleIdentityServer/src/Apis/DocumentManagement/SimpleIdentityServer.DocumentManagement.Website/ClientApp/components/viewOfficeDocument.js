import React, { Component } from "react";
import { translate } from 'react-i18next';
import { NavLink } from 'react-router-dom';
import { Checkbox, CircularProgress, IconButton, Grid, List, ListItem, ListItemText, Label } from 'material-ui';
import { withStyles } from 'material-ui/styles';
import { OfficeDocumentService } from '../services';
import Table, { TableBody, TableCell, TableHead, TableRow, TableFooter, TablePagination, TableSortLabel } from 'material-ui/Table';
import { FormControl, FormHelperText, TextField  } from 'material-ui/Form';
import Input, { InputLabel } from 'material-ui/Input';
import Visibility from '@material-ui/icons/Visibility'; 
import AppDispatcher from '../appDispatcher';
import Constants from '../constants';

const styles = theme => ({
  margin: {
    margin: theme.spacing.unit,
  }
});

class ViewOfficeDocument extends Component {
    constructor(props) {
        super(props);
        this.refreshData = this.refreshData.bind(this);
        this.state = {
            isLoading: true,
            id: null,
            officeDocument: {
                id: '',
                display_name: '',
                subject: ''
            }
        };
    }

    /**
    * Display the document information.
    */
    refreshData() {
        var self = this;
        const { t } = self.props;
        self.setState({
            isLoading: true
        });
        Promise.all([ OfficeDocumentService.getOfficeDocumentInformation(self.state.id), OfficeDocumentService.getPermissions(self.state.id) ]).then(function(values) {
            var officeDocument = values[0];
            var permissions = values[1];
            self.setState({
                isLoading: false,
                officeDocument: officeDocument,
                permissions: permissions
            });
        }).catch(function(e) {
            console.log(e);
            self.setState({
                isLoading: false,
                officeDocument: {
                    id: '',
                    display_name: '',
                    subject: ''
                },
                permissions: []
            });
            AppDispatcher.dispatch({
                actionName: Constants.events.DISPLAY_MESSAGE,
                data: t('cannotRetrieveOfficeDocument')
            });
        });
    }

    /**
    * Display view.
    */
    render() {
        var self = this;
        const { t, classes } = self.props;
        var listItems = [];
        if(self.state.permissions) {
            self.state.permissions.forEach(function(permission) {
                listItems.push(
                    <ListItem dense button style={{overflow: 'hidden'}}>
                        <ListItemText>{permission.sub}</ListItemText>
                    </ListItem>
                );
            });
        }

        return (<div className="block">
            <div className="block-header">
                <Grid container>
                    <Grid item md={5} sm={12}>
                        <h4>{t('officeDocumentTitle')}</h4>
                        <i>{t('officeDocumentShortDescription')}</i>
                    </Grid>
                    <Grid item md={7} sm={12}>                        
                        <ul className="breadcrumb float-md-right">
                            <li className="breadcrumb-item"><NavLink to="/">{t('websiteTitle')}</NavLink></li>
                            <li className="breadcrumb-item"><NavLink to="/documents">{t('officeDocumentsTitle')}</NavLink></li>
                            <li className="breadcrumb-item">{t('officeDocumentTitle')}</li>
                        </ul>
                    </Grid>
                </Grid>
            </div>
            <div className="card">
                <div className="header">
                    <h4 style={{display: "inline-block"}}>{t('officeDocument')}</h4>
                </div>
                <div className="body">
                    { self.state.isLoading ? (<CircularProgress />) : (
                        <Grid container>
                            <Grid item md={5} sm={12}>
                                <FormControl fullWidth={true} className={classes.margin} disabled={true}>
                                    <InputLabel>{t('documentId')}</InputLabel>
                                    <Input value={self.state.officeDocument.id}/>
                                </FormControl>     
                                <FormControl fullWidth={true} className={classes.margin} disabled={true}>
                                    <InputLabel>{t('documentName')}</InputLabel>
                                    <Input value={self.state.officeDocument.display_name}/>
                                </FormControl>     
                                <FormControl fullWidth={true} className={classes.margin} disabled={true}>
                                    <InputLabel>{t('documentSubject')}</InputLabel>
                                    <Input value={self.state.officeDocument.subject}/>
                                </FormControl>      
                            </Grid>          
                            <Grid item md={7} sm={12}>
                                <span>{t('listOfAuthorizedUsers')}</span>
                                <List>
                                    {listItems}
                                </List>
                            </Grid>          
                        </Grid>
                    )}
                </div>
            </div>
        </div>);
    }

    componentDidMount() {
        var self = this;
        self.setState({
            id: self.props.match.params.id
        }, function() {
            self.refreshData();
        });
    }
}

export default translate('common', { wait: process && !process.release })(withStyles(styles)(ViewOfficeDocument));