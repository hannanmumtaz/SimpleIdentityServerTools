import React, { Component } from "react";
import { translate } from 'react-i18next';
import { ParameterService } from '../services';
import { NavLink, withRouter } from "react-router-dom";
import moment from 'moment';
import AppDispatcher from '../appDispatcher';
import Constants from '../constants';

import { Popover, IconButton, Menu, Input, MenuItem, Checkbox, TextField, Select, Avatar , CircularProgress, Grid, Button, ExpansionPanel, ExpansionPanelSummary, ExpansionPanelDetails, Typography } from 'material-ui';
import Table, { TableBody, TableCell, TableHead, TableRow, TableFooter, TablePagination, TableSortLabel } from 'material-ui/Table';
import ExpandMoreIcon from '@material-ui/icons/ExpandMore';
import Save from '@material-ui/icons/Save';

class ViewSetting extends Component {
	constructor(props) {
		super(props);
		this.refresh = this.refresh.bind(this);
		this.saveSettings = this.saveSettings.bind(this);
		this.updatePropertyValue = this.updatePropertyValue.bind(this);
		this.state = {
			isLoading: false,
			id: null,
			units: []
		};
	}

	refresh() {
		var self = this;
		self.setState({
			isLoading: true
		});
		var groupBy = function(xs, key) {
		  return xs.reduce(function(rv, x) {
		    (rv[x[key]] = rv[x[key]] || []).push(x);
		    return rv;
		  }, {});
		};
		const {t} = self.props;
		ParameterService.getModules(self.state.id).then(function(result) {
			var units = [];
			result['template_units'].forEach(function(ut) {
				var record = {
					name: ut.name,
					packages: []
				};
				var pu = result['units'].filter(function(u) { return u.name === ut.name; })[0];
				var groupedUtPackages = groupBy(ut.packages, 'category');
				for(var kvp in groupedUtPackages)
				{
					groupedUtPackages[kvp].forEach(function(ut) {
						var filteredPackage = pu['packages'].filter(function(up) { return up.lib === ut.lib && up.category === kvp; });
						if (filteredPackage.length > 0) {
							ut.isSelected = true;
							ut.parameters = filteredPackage[0].parameters;
						} else {
							ut.isSelected = false;
						}
					});
				}

				record['packages'] = groupedUtPackages;
				units.push(record);
			});

			self.setState({
				isLoading: false,
				units: units
			});
		}).catch(function() {
            AppDispatcher.dispatch({
                actionName: Constants.events.DISPLAY_MESSAGE,
                data: t('settingsCannotBeRetrieved')
            });
			self.setState({
				isLoading: false
			});
		});
	}

	saveSettings() {

	}

	updatePropertyValue(e, pkg, name) {
		// TODO : Update the property value.
		/*
		var units = this.state.units;
		// pkg.parameters[name] = e.target.value;
		console.log(e.target.value);
		// console.log(pkg.parameters[name]);
		this.setState({
			units: units
		});
		*/
	}

	render() {
		var self = this;
		const {t} = self.props;
		var rows = [];
		if (self.state.units) {
			self.state.units.forEach(function(unit) {
				var packages = [];
				if (unit.packages) {
					for(var category in unit.packages)
					{
						var parameters = [];
						var pkgs = unit.packages[category];
						var options = [];
						var selectedPackage = null;
						var currentValue = "";
						pkgs.forEach(function(p) {
							if (p.isSelected) {
								selectedPackage = p;
								currentValue = p.lib;
							}

							options.push(<MenuItem value={p.lib}>{p.lib}_{p.version}</MenuItem>);
						});

						if (selectedPackage.parameters) {
							for(var pkgParameter in selectedPackage.parameters)
							{
								parameters.push(
									<TableRow>
										<TableCell>{pkgParameter}</TableCell>
										<TableCell>
											<Input value={selectedPackage.parameters[pkgParameter]} onChange={(e) => self.updatePropertyValue(e, selectedPackage, pkgParameter) } name={pkgParameter}  />
										</TableCell>
									</TableRow>
								);
							}
						}

						packages.push(
							<Grid container spacing={40}>
								<Grid item sm={12} md={6}>
									<Typography>{category}</Typography>
									<Select value={currentValue} fullWidth={true}>
										{options}
									</Select>
								</Grid>
								<Grid item s={12} md={6}>
									<Table>
										<TableHead>
											<TableRow>
												<TableCell>{t('parameterKey')}</TableCell>
												<TableCell>{t('parameterValue')}</TableCell>
											</TableRow>
										</TableHead>
										<TableBody>
											{parameters}
										</TableBody>
									</Table>
								</Grid>
							</Grid>
						);
					}
				}

				rows.push(
					<ExpansionPanel>
						<ExpansionPanelSummary expandIcon={<ExpandMoreIcon />}>
							{unit.name}
						</ExpansionPanelSummary>
						<ExpansionPanelDetails style={{ display: 'block' }}>
							{packages}
						</ExpansionPanelDetails>
					</ExpansionPanel>
				);
			});
		}

		return (<div className="block">
			<div className="block-header">
                <Grid container>
                    <Grid item md={5} sm={12}>
                        <h4>{t('moduleParameters')}</h4>
                        <i>{t('moduleParametersShortDescription')}</i>
                    </Grid>
                    <Grid item md={7} sm={12}>
                        <ul className="breadcrumb float-md-right">
                            <li className="breadcrumb-item"><NavLink to="/">{t('websiteTitle')}</NavLink></li>
                            <li className="breadcrumb-item"><NavLink to="/settings">{t('modules')}</NavLink></li>
                            <li className="breadcrumb-item">{t('parameters')}</li>
                        </ul>
                    </Grid>
                </Grid>
			</div>
			 <Grid container spacing={40}>
			 	 <Grid item md={12}>
					<div className="card">
						<div className="header">
		                    <h4 style={{display: "inline-block"}}>{t('moduleInformations')}</h4>
						</div>
						<div className="body">
							
						</div>
					</div>
			 	 </Grid>
			 	 <Grid item md={12}>
					<div className="card">
		                <div className="header">
		                    <h4 style={{display: "inline-block"}}>{t('moduleParameters')}</h4>
		                    <div style={{float: "right"}}>                        
		                        <IconButton onClick={this.saveSettings}>
		                            <Save />
		                        </IconButton>
		                    </div>
		                </div>
		                <div className="body">
		                	{ this.state.isLoading ? (<CircularProgress />) : (
		                		<div>
		                			{rows}
		                		</div>
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

export default translate('common', { wait: process && !process.release })(withRouter(ViewSetting));