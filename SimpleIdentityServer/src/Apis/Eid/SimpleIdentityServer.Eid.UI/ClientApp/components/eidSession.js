import React from "react";
import BaseComponent from '../baseComponent';
import Modal from './modal';
import moment from 'moment';
import { SessionService } from '../services';

class EidSession extends BaseComponent {
    constructor(props) {
        super(props);
        this.interval = null;
        this.myModal = null;
        this.toggleSession = this.toggleSession.bind(this);
        this.hideModal = this.hideModal.bind(this);
        this.connect = this.connect.bind(this);
        this.getSession = this.getSession.bind(this);
        this.updateExpirationDate = this.updateExpirationDate.bind(this);
        this.state = {
            expirationDate: null,
            expirationTime: null,
            pin: null,
            type: 'patient',
            isModalDisplayed: false,
            isLoading: false,
            errorMessage: null,
            types: [
                { id: 'patient', displayName: 'patient' },
                { id: 'doctor', displayName: 'doctor' }
            ]
        };
    }

    /* Hide the modal window */
    hideModal() {
        this.setState({
            isModalDisplayed: false
        });
    }

    /* Update expiration date */
    updateExpirationDate() {
        if (!this.state.expirationDate) {
            return;
        }

        var expiresDate = moment(moment(this.state.expirationDate).toDate());
        var currentDate = moment();
        var diffValue = expiresDate.diff(currentDate);
        /*
        if (diffValue >= 0) {
            this.setState({
                expirationTime: '1 jour'
            });
            return;
        }
        */

        var diff = moment.utc(diffValue).format("HH:mm:ss");
        this.setState({
            expirationTime: diff
        });
    }

    /* Toggle the session */
    toggleSession() {
        var self = this;
        if (!self.state.expirationDate) {
            self.setState({
                isModalDisplayed: true
            });
            return;
        }

        self.setState({
            isLoading: true
        });
        SessionService.removeSession().then(function (session) {
            if (self.interval) { clearInterval(self.interval); }
            self.setState({
                expirationDate: null,
                expirationTime: null,
                isLoading: false,
                errorMessage: null
            });
        }).catch(function () {
            self.setState({
                expirationDate: null,
                expirationTime: null,
                isLoading: false,
                errorMessage: null
            });
        });
    }

    /* Connect to the BEID card */
    connect() {
        var self = this;
        if (!self.myModal) {
            return;
        }

        self.myModal.close();
        self.setState({
            isModalDisplayed: false
        });
        var code = self.state.pin;
        self.setState({
            isLoading: true
        });
        // TH : Check the code is well formatted.        
        SessionService.createSession(code, this.state.type).then(function () {
            self.getSession();
        }).catch(function (e) {
            self.setState({
                isLoading: false,
                errorMessage: e.message
            });
        });
    }

    /* Get the BEID session */
    getSession() {
        var self = this;
        self.setState({
            isLoading: true
        });
        SessionService.getSession().then(function (session) {
            self.interval = setInterval(() => self.updateExpirationDate(), 1000);
            self.setState({
                expirationDate: session.expires,
                expirationTime: null,
                isLoading: false,
                errorMessage: null
            });
        }).catch(function () {
            self.setState({
                expirationDate: null,
                expirationTime: null,
                isLoading: false,
                errorMessage: null
            });
        });
    }

    render() {
        var options = [];
        if (this.state.types) {
            this.state.types.forEach(function (type) {
                options.push((<option value={type.id}>{type.displayName}</option>));
            });
        }

        return (<div>
            { this.state.isLoading && (
                <div>
                    <div className="loader-overlay"></div>
                    <div className="loader-spinner fixed">
                        <h2 className="font-secondary redColor">Authentification...</h2>
                        <img src="/images/loader-spinner.gif" />
                    </div>
                </div>                
            )}
            { this.state.errorMessage && (
                <div className="alert alert-danger" role="alert">
                    <strong>Erreur !</strong> {this.state.errorMessage}
                </div>
            )}
            {this.state.isModalDisplayed && (<Modal title='Authentifier' ref={c => this.myModal = c} handleHideModal={this.hideModal} onConfirmHandle={this.connect}  confirmTxt="Se connecter" >
                <form onSubmit={this.connect}>
                    <div className="form-group">
                        <label>Votre role</label>
                        <select className="form-control" onChange={this.handleInputChange} name="type">
                            {options}
                        </select>
                    </div>
                    <div className="form-group">
                        <label>Code PIN</label>
                        <input type="password" pattern="[0-9]{4}" className="form-control" name="pin" onChange={this.handleInputChange} />
                    </div>  
                </form>
            </Modal>)}
            <div>
                { this.state.expirationTime ?
                    (<label className="control-label">Une session est active et expire dans {this.state.expirationTime}</label>) : 
                    (<label className="control-label">Aucune session n'est active</label>) }
            </div>
            <div>
                <button className="btn btn-default" onClick={this.toggleSession}>{this.state.expirationDate ? "Deconnecter" : "Connecter"}</button>
            </div>
		</div>);
    }

    componentDidMount() {
        this.getSession();
    }
}

export default EidSession;