import React, { Component } from "react";
import { translate } from 'react-i18next';
import Table, { TableBody, TableCell, TableHead, TableRow, TableFooter } from 'material-ui/Table';
import { Grid, IconButton, CircularProgress, Checkbox } from 'material-ui';
import Done from '@material-ui/icons/Done';

class Users extends Component {
    constructor(props) {
        super(props);
        this.enableAccounts = this.enableAccounts.bind(this);
        this.refresh = this.refresh.bind(this);
        this.handleRowClick = this.handleRowClick.bind(this);
        this.handleAllSelections = this.handleAllSelections.bind(this);
        this.state = {
            users: [],
            isConfirmedDispayed: false,
            isLoading: false
        };
    }

    enableAccounts() {

    }

    refresh() {
        var self = this;
        self.setState({
            isLoading: true
        });
        self.setState({
            users: [
                { subject: '01', name: 'thabart', isChecked: false }
            ]
        });
        self.setState({
            isLoading: false
        });
    }

    handleRowClick(e, user) {
        user.isChecked = e.target.checked;
        var users = this.state.users;
        var nbSelectedRecords = users.filter(function (r) { return r.isChecked; }).length;
        this.setState({
            users: users,
            isConfirmedDispayed: nbSelectedRecords > 0
        });
    }

    handleAllSelections(e) {
        var checked = e.target.checked;
        var users = this.state.users;
        users.forEach(function (r) { r.isChecked = checked; });
        this.setState({
            users: users,
            isConfirmedDispayed: checked
        });
    }

    render() {
        const { t } = this.props;
        var self = this;
        var rows = [];
        if (self.state.users) {
            self.state.users.forEach(function (user) {
                rows.push(
                    <TableRow key={user.subject}>
                        <TableCell><Checkbox color="primary" checked={user.isChecked} onChange={(e) => self.handleRowClick(e, user)} /></TableCell>
                        <TableCell>{user.subject}</TableCell>
                        <TableCell>{user.name}</TableCell>
                    </TableRow>
                );
            });
        }

        return (<div className="block">
            <div className="block-header">
                <Grid container>
                    <Grid item md={5} sm={12}>
                        <h4>{t('usersTitle')}</h4>
                        <i>{t('usersDescription')}</i>
                     </Grid>
                     <Grid item md={7} sm={12}>               
                        <ul className="breadcrumb float-md-right">
                            <li className="breadcrumb-item">{t('websiteTitle')}</li>
                            <li className="breadcrumb-item">{t('users')}</li>
                        </ul>
                     </Grid>
                </Grid>
            </div>
            <div className="card">
                <div className="header">
                    <h4 style={{ display: "inline-block" }}>{t('listOfUsers')}</h4>
                    <div style={{ float: "right" }}>
                        {self.state.isConfirmedDispayed && (
                            <IconButton onClick={self.enableAccounts}>
                                <Done />
                            </IconButton>
                        )}
                    </div>
                </div>
                <div className="body">
                    {self.state.isLoading ? (<CircularProgress />) : (
                        <div>
                            <Table>
                                <TableHead>
                                    <TableRow>
                                        <TableCell></TableCell>
                                        <TableCell>{t('subject')}</TableCell>
                                        <TableCell>{t('name')}</TableCell>
                                    </TableRow>
                                </TableHead>
                                <TableBody>
                                    <TableRow>
                                        <TableCell><Checkbox color="primary" onChange={self.handleAllSelections} /></TableCell>
                                        <TableCell></TableCell>
                                        <TableCell></TableCell>
                                    </TableRow>
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
        this.refresh();
    }
}

export default translate('common', { wait: process && !process.release })(Users);