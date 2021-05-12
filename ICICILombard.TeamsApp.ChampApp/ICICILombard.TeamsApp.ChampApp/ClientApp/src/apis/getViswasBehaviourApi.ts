import axios from 'axios';
import {getUrl,routers} from './Endpoint'

export const getViswasBehaviourAPI = async (token: any) => {
    console.log('In api',  getUrl(routers.behaviours))
    return await axios.get(getUrl(routers.behaviours),{headers: {"Authorization" : "Bearer "+ token}});
}
