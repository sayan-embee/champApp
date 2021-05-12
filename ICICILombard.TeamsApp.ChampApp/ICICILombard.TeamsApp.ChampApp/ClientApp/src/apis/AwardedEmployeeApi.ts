import axios from 'axios';
import {getUrl,routers} from './Endpoint'

export const getAwardedEmployeeAPI = async () => {
    console.log('In api',  getUrl(routers.getawardedemployee))
    return await axios.get(getUrl(routers.getawardedemployee));
}
