import $ from 'jquery';
import Constants from '../constants';
import SessionService from './sessionService';

module.exports = {
    /**
    * Get the schemas.
    */
    getSchemas: function() {
        return new Promise(function (resolve, reject) {
            var session = SessionService.getSession();
            $.ajax({
                url: Constants.apiUrl + '/scim/schemas',
                method: "GET",
                headers: {
                    "Authorization": "Bearer "+ session.token
                }
            }).then(function (data) {
                resolve(data);
            }).fail(function (e) {
                reject(e);
            });
        });        
    }
};