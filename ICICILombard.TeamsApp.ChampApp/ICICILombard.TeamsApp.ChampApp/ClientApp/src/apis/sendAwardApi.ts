import axios from 'axios';
import {getUrl,routers} from './Endpoint'

export const sendAwardAPI = async (data: any) => {
    console.log('In api',  getUrl(routers.sendaward),data)
    return await axios.post(getUrl(routers.sendaward),data);
}
