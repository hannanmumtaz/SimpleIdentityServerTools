import React, { Component } from "react";
import AppDispatcher from '../appDispatcher';
import Constants from '../constants';
import { ScimService } from '../services';
import { withRouter } from 'react-router-dom';
import { NavLink } from 'react-router-dom';
import { translate } from 'react-i18next';
import { withStyles } from 'material-ui/styles';
import { SessionStore } from '../stores';
import { TextField , Button, Grid, IconButton, CircularProgress } from 'material-ui';
import Table, { TableBody, TableCell, TableHead, TableRow, TableFooter, TablePagination, TableSortLabel } from 'material-ui/Table';
import { FormControl, FormHelperText } from 'material-ui/Form';
import Input, { InputLabel } from 'material-ui/Input';
import Visibility from '@material-ui/icons/Visibility'; 

const styles = theme => ({
  margin: {
    margin: theme.spacing.unit,
  }
});

class ViewScimResource extends Component {
    constructor(props) {
        super(props);
        this.refresh = this.refresh.bind(this);
        this.state = {
            id: null,
            type: 'user',
            isLoading: false,
            resource: {}
        };
    }

    /**
    * Display the resource.
    */
    refresh() {
        var self = this;
        self.setState({
            isLoading: true
        });
        var opt = {};
        if (self.state.type === 'user') {
            opt = ScimService.getUser(self.state.id);
        } else {
            opt = ScimService.getGroup(self.state.id);
        }

        opt.then(function(result) {
            self.setState({
                isLoading: false,
                resource: result
            });
        }).catch(function() {
            AppDispatcher.dispatch({
                actionName: Constants.events.DISPLAY_MESSAGE,
                data: t('resourceCannotBeRetrieved')
            });
            self.setState({
                isLoading: false,
                resource: {}
            });
        });
    }

    render() {
        var self = this;
        const { t, classes } = self.props;
        var rows = [];
        if (self.state.resource) {
            for(var record in self.state.resource) {
                rows.push(
                    <FormControl key={record} fullWidth={true} className={classes.margin} disabled={true}>
                        <InputLabel>{record}</InputLabel>
                        <Input value={JSON.stringify(self.state.resource[record])} name="name"  />
                    </FormControl>
                );
            }
        }

        return (<div className="block">
            <div className="block-header">
                <Grid container>
                    <Grid item md={5} sm={12}>
                        <h4>{t('scimResource')}</h4>
                        <i>{t('scimResourceDescription')}</i>
                    </Grid>
                    <Grid item md={7} sm={12}>                        
                        <ul className="breadcrumb float-md-right">
                            <li className="breadcrumb-item"><NavLink to="/">{t('websiteTitle')}</NavLink></li>
                            <li className="breadcrumb-item"><NavLink to="/scim/resources">{t('scimResources')}</NavLink></li>
                            <li className="breadcrumb-item">{t('scimResource')}</li>
                        </ul>
                    </Grid>
                </Grid>
            </div>
            <div className="card">
                { self.state.isLoading ? ( <CircularProgress /> ) : (
                    <div>
                        <div className="header">
                            <h4 style={{display: "inline-block"}}>{t('scimResource')}</h4>
                        </div>
                        <div className="body">
                            {rows}
                        </div>
                    </div>
                )}
            </div>
        </div>);
    }

    componentDidMount() {
        var self = this;
        var type = self.props.match.params.type;
        if (type !== 'group' && type !== 'user') {
            type = 'user';
        }

        SessionStore.addChangeListener(function() {
            self.setState({            
                id: self.props.match.params.id,
                type: type
            }, function() {
                self.refresh();
            });
        });

        if (SessionStore.getSession().selectedOpenid) {  
            self.setState({            
                id: self.props.match.params.id,
                type: type
            }, function() {
                self.refresh();
            });
        }
    }
}

export default translate('common', { wait: process && !process.release })(withRouter(withStyles(styles)(ViewScimResource)));