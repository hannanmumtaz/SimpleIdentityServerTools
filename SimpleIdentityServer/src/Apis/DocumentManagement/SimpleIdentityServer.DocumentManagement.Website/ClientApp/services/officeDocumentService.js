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
	}
};