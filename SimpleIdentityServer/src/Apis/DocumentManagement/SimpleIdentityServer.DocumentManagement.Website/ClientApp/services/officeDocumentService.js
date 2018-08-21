import $ from 'jquery';
import SessionService from './sessionService';
import Constants from '../constants';

module.exports = {
	/**
	* Confirm the code
	*/
	confirmCode: function(confirmationCode) {
        return new Promise(function (resolve, reject) {
            var session = SessionService.getSession();
            $.ajax({
            	url: Constants.documentManagementApiUrl + '/officedocuments/' + confirmationCode + '/invitation/confirm' ,
            	method: 'GET',            	
                headers: {
                	"Authorization": "Bearer "+ session.token
                }
            }).then(function() {
            	resolve();
            }).catch(function() {
            	reject();
            });
        });
	},
    /**
    * Get the invitation link information.
    */
    getInvitationLinkInformation: function(confirmationCode) {
        return new Promise(function (resolve, reject) {
            $.ajax({
                url: Constants.documentManagementApiUrl + '/officedocuments/' + confirmationCode + '/invitation' ,
                method: 'GET'
            }).then(function(result) {
                resolve(result);
            }).catch(function() {
                reject();
            });
        });
    },
    /**
    * Get the office document information.
    */
    getOfficeDocumentInformation : function(documentId) {
        return new Promise(function (resolve, reject) {
            $.ajax({
                url: Constants.documentManagementApiUrl + '/officedocuments/' + documentId,
                method: 'GET'
            }).then(function(result) {
                resolve(result);
            }).catch(function() {
                reject();
            });
        });
    }
};