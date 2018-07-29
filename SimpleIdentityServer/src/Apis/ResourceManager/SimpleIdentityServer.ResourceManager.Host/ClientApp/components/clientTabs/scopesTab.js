import React, { Component } from "react";
import { translate } from 'react-i18next';
import { withRouter, NavLink } from 'react-router-dom';
import Table, { TableBody, TableCell, TableHead, TableRow, TableFooter, TablePagination, TableSortLabel} from 'material-ui/Table';
import { Checkbox, CircularProgress, Hidden, List, ListItem, ListItemText, IconButton  } from 'material-ui';
import { ScopeService } from '../../services';
import moment from 'moment';
import Visibility from '@material-ui/icons/Visibility'; 

class ScopesTab extends Component {
	constructor(props) {
		super(props);
		this.refreshData = this.refreshData.bind(this);
		this.handleAllSelections = this.handleAllSelections.bind(this);
        this.refreshAllowedScopes = this.refreshAllowedScopes.bind(this);
        this.handleRowClick = this.handleRowClick.bind(this);
		this.state = {
			isLoading: true,
			data: [],
            allowedScopes: props.allowedScopes
		};
	}

	/**
	* Display the scopes.
	*/
	refreshData() {
		var self = this;
        const { t } = self.props;
		self.setState({
			isLoading: true
		});

        var request = { start_index: 0, count: 3000, order: { target: 'update_datetime', type: 1 } };
		ScopeService.search(request, self.props.type).then(function(result) {
            var data = [];
            if (result.content) {
                result.content.forEach(function (scope) {
                    var type = scope.type;
                    type = type === 1 ? t('openidScope') : t('apiScope');
                    data.push({
                        name: scope['name'],
                        type: type,
                        isSelected: self.state.allowedScopes.indexOf(scope['name']) > -1
                    });
                });
            }
            
            self.setState({
                isLoading: false,
                data: data
            });
		}).catch(function (e) {
            self.setState({
                isLoading: false,
                data: []
            });
        });
	}

    /**
    * Select / Unselect all scopes.
    */
    handleAllSelections(e) {
        var self = this;
        var checked = e.target.checked;
        var data = self.state.data;
        data.forEach(function(r) { r.isSelected = checked ;});
        self.setState({
            data: data
        }, function() {
            self.refreshAllowedScopes();
        });
    }

    /**
    * Handle click on the row.
    */
    handleRowClick(e, record) {
        var self = this;
        record.isSelected = e.target.checked;
        var data = self.state.data;
        var nbSelectedRecords = data.filter(function(r) { return r.isSelected; }).length;
        self.setState({
            data: data
        }, function() {
            self.refreshAllowedScopes();
        });
    }

    /**
    * Refresh the allowed scopes.
    */
    refreshAllowedScopes() {
        var allowedScopes = this.state.allowedScopes;
        var scopeNames = this.state.data.filter(function(r)  { return r.isSelected; }).map(function(r) { return r.name; });
        scopeNames.forEach(function(scopeName) {
            if (allowedScopes.indexOf(scopeName) === -1) {
                allowedScopes.push(scopeName);
            }
        });       
        var removedIndexes = [];
        allowedScopes.forEach(function(scopeName) {
            if (scopeNames.indexOf(scopeName) === -1) {
                removedIndexes.push(allowedScopes.indexOf(scopeName));
            }
        });

        removedIndexes = removedIndexes.sort(function(a, b) {
            return b-a;
        });
        for (var i = 0; i < removedIndexes.length; i++) {
            allowedScopes.splice(removedIndexes[i], 1);
        }

        this.setState({
            allowedScopes: allowedScopes
        });
    }

	render() {
		var self = this;
		const { t } = self.props;
        if (self.state.isLoading) {
            return (<CircularProgress />);
        }

        var rows = [], listItems = [];
        if (self.state.data) {
            self.state.data.forEach(function(record) {
                rows.push((
                    <TableRow hover role="checkbox" key={record.name}>
                        <TableCell><Checkbox color="primary" checked={record.isSelected} onChange={(e) => self.handleRowClick(e, record)} /></TableCell>
                        <TableCell>{record.name}</TableCell>
                        <TableCell>{record.type}</TableCell>
                        <TableCell>
                            <NavLink to={'/' + self.props.type + '/scopes/' + record.name}>
                                <IconButton>
                                    <Visibility />
                                </IconButton>
                            </NavLink>
                        </TableCell>
                    </TableRow>
                ));
                listItems.push(
                    <ListItem dense button style={{overflow: 'hidden'}}>
                        <Checkbox color="primary" checked={record.isSelected} onChange={(e) => self.handleRowClick(e, record)} />
                        <NavLink to={'/' + self.props.type + '/scopes/' + record.name}>
                            <IconButton>
                                <Visibility />
                            </IconButton>
                        </NavLink>
                        <ListItemText>{record.name}</ListItemText>
                    </ListItem>
                );
            });
        }
        
		return (
            <div>
                <Hidden only={['xs', 'sm']}>
        			<Table>
                    	<TableHead>
                        	<TableRow>
                            	<TableCell><Checkbox color="primary" onChange={self.handleAllSelections} /></TableCell>
                                <TableCell>{t('scopeName')}</TableCell>
                                <TableCell>{t('scopeType')}</TableCell>
                                <TableCell></TableCell>
                            </TableRow>
                        </TableHead>
                        <TableBody>
                            {rows}
                        </TableBody>
                        <TableFooter>
                        </TableFooter>
                   </Table>
                </Hidden>
                <Hidden only={['lg', 'xl', 'md']}>
                    <List>
                        {listItems}
                    </List>
                </Hidden>
           </div>
		);
	}

	componentDidMount() {
		this.refreshData();
	}
}

export default translate('common', { wait: process && !process.release })(ScopesTab);