import axios from 'axios';
import {getUrl,routers} from './Endpoint'

export const getBadgesAPI = async (token: any) => {
    console.log('In api',  getUrl(routers.badges))
    return await axios.get(getUrl(routers.badges),{headers: {"Authorization" : "Bearer "+ token}});
}
