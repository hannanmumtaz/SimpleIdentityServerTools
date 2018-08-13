import $ from 'jquery';
import Constants from '../constants';

module.exports = {
	/**
	* Get all the authentication providers.
	*/
	getAuthProviders: function() {
        return new Promise(function (resolve, reject) {
            $.ajax({
                url: "http://localhost:60000/authproviders", // TODO : EXTERNALIZE THIS URL.
                method: "GET"
            }).then(function (data) {
                resolve(data);
            }).fail(function (e) {
                reject(e);
            });
		});
	}
};