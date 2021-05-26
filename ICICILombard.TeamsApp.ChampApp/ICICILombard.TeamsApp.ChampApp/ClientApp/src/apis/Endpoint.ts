const base_URL = window.location.origin+'/api/champ-app/';  

export const routers = {
    badges: 'badges',
    behaviours:'viswas-behaviour',
    teammember:'teammembers?teamId=',
    chatmember:'chatmembers?chatId=',
    sendaward:'sendaward',
    addapplausecard: 'addapplausecard',
    getapplausecard:'getapplausecard',
    getbehaviours:'getvishvasbehaviour',
    addbehaviours:'addvishvasbehaviour',
    getappsetting:'getappsetting',
    updateappsetting:'updateappsetting',
    getawardlist:"getawardlist",
    getawardedemployee:"getawardedemployee",
    getawardlistbycard:"getawardlistbycard",
    getawardlistbyrecipent:"getawardlistbyrecipent"
}
export const getUrl = (key: any) => {
    return base_URL + key;
} 
