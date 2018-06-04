import React, { Component } from "react";
import { translate } from 'react-i18next';
import { ResourceOwnerService } from '../services';
import { NavLink, withRouter } from "react-router-dom";
import moment from 'moment';

import Table, { TableBody, TableCell, TableHead, TableRow, TableFooter, TablePagination, TableSortLabel } from 'material-ui/Table';
import { Popover, IconButton, Menu, MenuItem, Checkbox, TextField, Select, Avatar , CircularProgress, Grid, Button } from 'material-ui';
import Dialog, { DialogTitle, DialogContent, DialogActions } from 'material-ui/Dialog';
import Input, { InputLabel } from 'material-ui/Input';
import { FormControl, FormHelperText } from 'material-ui/Form';

class Settings extends Component {
	constructor(props) {
		super(props);
		this.refresh = this.refresh.bind(this);
		this.state = {
			isLoading: false,
			modules: []
		};
	}

	/**
	* Display the settings.
	*/
	refresh() {
		var self = this;
		self.setState({
			isLoading: true
		});
	}

	render() {
		var self = this;
		const {t} = self.props;
		var rows = [];
		return (<div className="block">
			<div className="block-header">
                <Grid container>
                    <Grid item md={5} sm={12}>
                        <h4>{t('modules')}</h4>
                        <i>{t('modulesShortDescription')}</i>
                    </Grid>
                    <Grid item md={7} sm={12}>
                        <ul className="breadcrumb float-md-right">
                            <li className="breadcrumb-item"><NavLink to="/">{t('websiteTitle')}</NavLink></li>
                            <li className="breadcrumb-item">{t('modules')}</li>
                        </ul>
                    </Grid>
                </Grid>
			</div>
			<div className="card">
                <div className="header">
                    <h4 style={{display: "inline-block"}}>{t('listOfModules')}</h4>
                </div>
                <div className="body">
                	{ this.state.isLoading ? (<CircularProgress />) : (
                		<div>
                			<Table>
                				<TableHead>
                					<TableRow>
                						<TableCell>{t('moduleName')}</TableCell>
                						<TableCell></TableCell>
                					</TableRow>
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

	}
}

export default translate('common', { wait: process && !process.release })(withRouter(Settings));