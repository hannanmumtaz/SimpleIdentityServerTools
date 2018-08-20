import React, { Component } from "react";
import { translate } from 'react-i18next';
import { Grid } from 'material-ui';
import { NavLink } from 'react-router-dom';
import { withStyles } from 'material-ui/styles';
import { FormControl, FormHelperText } from 'material-ui/Form';
import Input, { InputLabel } from 'material-ui/Input';

const styles = theme => ({
  margin: {
    margin: theme.spacing.unit,
  }
});

class ConfirmActivationCode extends Component {
    constructor(props) {
	super(props);
	this.state = {
		code : null
	};
    }

    render() {
        var self = this;
        const { t, classes } = self.props;
        return (<div className="block">
            <div className="block-header">
                <Grid container>
                    <Grid item md={5} sm={12}>
                        <h4>{t('confirmActivationCodeTitle')}</h4>
                        <i>{t('confirmActivationCodeShortDescription')}</i>
                    </Grid>
                    <Grid item md={7} sm={12}>                        
                        <ul className="breadcrumb float-md-right">
                            <li className="breadcrumb-item"><NavLink to="/">{t('websiteTitle')}</NavLink></li>
                            <li className="breadcrumb-item">{t('confirmActivationCodeTitle')}</li>
                        </ul>
                    </Grid>
                </Grid>
            </div>
            <div className="card">
                <div className="header">
                    <h4 style={{display: "inline-block"}}>{t('confirmActivationCode')}</h4>
                </div>
                <div className="body">
			<FormControl fullWidth={true} className={classes.margin} disabled={true}>				
                                <InputLabel>{t('confirmationCode')}</InputLabel>
                                <Input value={self.state.code} />
			</FormControl>
                </div>
            </div>
        </div>);
    }
    
    componentDidMount() {
	var self = this;
	self.setState({
		code: self.props.match.params.code
        });
    }
}

export default translate('common', { wait: process && !process.release })(withStyles(styles)(ConfirmActivationCode));