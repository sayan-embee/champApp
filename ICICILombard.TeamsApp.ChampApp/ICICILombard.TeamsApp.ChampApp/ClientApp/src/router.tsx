import Badges from './messagingExtentionComponents/badge';
import Details from './messagingExtentionComponents/details'
import Preview from './messagingExtentionComponents/preview'
import Vishvas from './admin/pages/viswasPage'
import Setting from './admin/pages/settingsPage'
import Badge from './admin/pages/badgePage'
import AwardPage from './admin/pages/awardPage'
import AdminInitial from './admin/adminInitial'
import AdminPreview from './admin/adminPreview'
import ReportInitial from './admin/reportInitial'

export const Routes=[
    {path:'/badge', component:Badges},
    {path:'/details', component:Details},
    {path:'/preview', component:Preview},
    {path:'/admin_viswas', component:Vishvas},
    {path:'/admin_setting', component:Setting},
    {path:'/admin_card', component:Badge},
    {path:'/admin_award', component:AwardPage},
    {path:'/admin_initial', component:AdminInitial},
    {path:'/admin_preview', component:AdminPreview},
    {path:'/report_initial', component:ReportInitial},

    {path:'/', exact:true, redirectTo:'/admin_preview'},
]