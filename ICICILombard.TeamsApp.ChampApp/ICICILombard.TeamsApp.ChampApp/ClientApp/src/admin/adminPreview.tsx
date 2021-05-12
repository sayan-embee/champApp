import React from 'react';
import { Header, Flex, Button, Card, CardBody} from "@fluentui/react-northstar";
import { ArrowRightIcon } from '@fluentui/react-icons-northstar';
import "./../styles.scss"


interface IPreviewProps {
    history?: any;
    location?: any
}

interface IPreviewState {
    
}

class AdminPreview extends React.Component<IPreviewProps, IPreviewState> {

    constructor(props: IPreviewProps) {
        super(props);
        this.state = {
            allData: [],
            selectedBadgeDetails:[]
        };
    }




    componentDidMount() {
        
    }

    viswas(){
      this.props.history.push(`/admin_viswas`)
    }

    setting(){
      this.props.history.push(`/admin_setting`)
    }

    card(){
      this.props.history.push(`/admin_card`)
    }

    render() {
        return (
          <div>
          <Flex vAlign="center" hAlign="center" gap="gap.small" padding="padding.medium">
            <Flex gap="gap.small" padding="padding.medium">
              <Card size="small" styles={{
                borderRadius: '6px',
                padding: '10px 20px 40px',
                boxShadow: '0px 10px 13px rgb(0 0 0 / 2%)',
                cursor: 'pointer',
              }}>
                <CardBody></CardBody>
                <svg xmlns="http://www.w3.org/2000/svg" width="57.667" height="57.667" viewBox="0 0 57.667 57.667">
                  <g id="_2422208" data-name="2422208" transform="translate(-16 -16)">
                    <path id="Path_176" data-name="Path 176" d="M79.745,88v46.133a1.922,1.922,0,0,1-1.922,1.922h-51.9A1.922,1.922,0,0,1,24,134.133V88Z" transform="translate(-7.039 -63.35)" fill="#e0edff" />
                    <path id="Path_177" data-name="Path 177" d="M24,31.689V25.922A1.922,1.922,0,0,1,25.922,24h51.9a1.922,1.922,0,0,1,1.922,1.922v5.767Z" transform="translate(-7.039 -7.039)" fill="#e0edff" />
                    <g id="Group_148" data-name="Group 148" transform="translate(21.767 26.572)">
                      <path id="Path_178" data-name="Path 178" d="M216,224h3.844v3.844H216Z" transform="translate(-197.739 -209.583)" fill="#548aff" />
                      <path id="Path_179" data-name="Path 179" d="M216,288h3.844v3.844H216Z" transform="translate(-197.739 -265.894)" fill="#548aff" />
                      <path id="Path_180" data-name="Path 180" d="M216,352h3.844v3.844H216Z" transform="translate(-197.739 -322.206)" fill="#548aff" />
                      <path id="Path_181" data-name="Path 181" d="M216,416h3.844v3.844H216Z" transform="translate(-197.739 -378.517)" fill="#548aff" />
                      <path id="Path_182" data-name="Path 182" d="M64,104v41.328H77.456V113.611h32.678V104Z" transform="translate(-64 -104)" fill="#548aff" />
                    </g>
                    <path id="Path_183" data-name="Path 183" d="M70.783,16h-51.9A2.887,2.887,0,0,0,16,18.883v51.9a2.887,2.887,0,0,0,2.883,2.883h51.9a2.887,2.887,0,0,0,2.883-2.883v-51.9A2.887,2.887,0,0,0,70.783,16Zm.961,54.783a.962.962,0,0,1-.961.961h-51.9a.962.962,0,0,1-.961-.961v-51.9a.962.962,0,0,1,.961-.961h51.9a.962.962,0,0,1,.961.961Z" transform="translate(0 0)" />
                    <ellipse id="Ellipse_34" data-name="Ellipse 34" cx="0.834" cy="1.113" rx="0.834" ry="1.113" transform="translate(19.894 19.708)" />
                    <circle id="Ellipse_35" data-name="Ellipse 35" cx="1.113" cy="1.113" r="1.113" transform="translate(22.675 19.708)" />
                    <circle id="Ellipse_36" data-name="Ellipse 36" cx="1.113" cy="1.113" r="1.113" transform="translate(25.457 19.708)" />
                    <path id="Path_184" data-name="Path 184" d="M212.806,216h-3.844a.961.961,0,0,0-.961.961v3.844a.961.961,0,0,0,.961.961h3.844a.961.961,0,0,0,.961-.961v-3.844a.961.961,0,0,0-.961-.961Zm-.961,3.844h-1.922v-1.922h1.922Z" transform="translate(-168.933 -175.972)" />
                    <path id="Path_185" data-name="Path 185" d="M280,232h2.883v1.922H280Z" transform="translate(-232.283 -190.05)" />
                    <path id="Path_186" data-name="Path 186" d="M320,232h13.456v1.922H320Z" transform="translate(-267.478 -190.05)" />
                    <path id="Path_187" data-name="Path 187" d="M212.806,280h-3.844a.961.961,0,0,0-.961.961v3.844a.961.961,0,0,0,.961.961h3.844a.961.961,0,0,0,.961-.961v-3.844a.961.961,0,0,0-.961-.961Zm-.961,3.844h-1.922v-1.922h1.922Z" transform="translate(-168.933 -232.283)" />
                    <path id="Path_188" data-name="Path 188" d="M280,296h2.883v1.922H280Z" transform="translate(-232.283 -246.361)" />
                    <path id="Path_189" data-name="Path 189" d="M320,296h13.456v1.922H320Z" transform="translate(-267.478 -246.361)" />
                    <path id="Path_190" data-name="Path 190" d="M212.806,344h-3.844a.961.961,0,0,0-.961.961v3.844a.961.961,0,0,0,.961.961h3.844a.961.961,0,0,0,.961-.961v-3.844a.961.961,0,0,0-.961-.961Zm-.961,3.844h-1.922v-1.922h1.922Z" transform="translate(-168.933 -288.594)" />
                    <path id="Path_191" data-name="Path 191" d="M280,360h2.883v1.922H280Z" transform="translate(-232.283 -302.672)" />
                    <path id="Path_192" data-name="Path 192" d="M320,360h13.456v1.922H320Z" transform="translate(-267.478 -302.672)" />
                    <path id="Path_193" data-name="Path 193" d="M212.806,408h-3.844a.961.961,0,0,0-.961.961v3.844a.961.961,0,0,0,.961.961h3.844a.961.961,0,0,0,.961-.961v-3.844a.961.961,0,0,0-.961-.961Zm-.961,3.844h-1.922v-1.922h1.922Z" transform="translate(-168.933 -344.905)" />
                    <path id="Path_194" data-name="Path 194" d="M280,424h2.883v1.922H280Z" transform="translate(-232.283 -358.983)" />
                    <path id="Path_195" data-name="Path 195" d="M320,424h13.456v1.922H320Z" transform="translate(-267.478 -358.983)" />
                    <path id="Path_196" data-name="Path 196" d="M103.094,96H56.961a.961.961,0,0,0-.961.961v41.328a.961.961,0,0,0,.961.961H70.417a.961.961,0,0,0,.961-.961V107.533h31.717a.961.961,0,0,0,.961-.961V96.961a.961.961,0,0,0-.961-.961ZM69.456,137.328H57.922V107.533H69.456Zm32.678-31.717H57.922V97.922h44.211Z" transform="translate(-35.194 -70.389)" />
                  </g>
                </svg>
  
                <Header as="h4" content=" Vishvas Behaviours" style={{
                  paddingTop: '20px',
                  marginBottom: '0'
                }} />
                {/* <Button  fluid circular icon={<ArrowRightIcon />} text title="Create" styles={{
                  textAlign:'right'
                  
                }} /> */}
                <Flex hAlign="end" vAlign="end" gap="gap.smaller">
                  <Button className="curve-btn" icon={<ArrowRightIcon />} title="Microphone" onClick={()=>this.viswas()} />
                </Flex>
              </Card>
            </Flex>
            <Flex gap="gap.small" padding="padding.medium">
              <Card size="small" styles={{
                borderRadius: '6px',
                padding: '10px 20px 40px',
                backgroundColor: '#ffffff',
                boxShadow: '0px 10px 13px rgb(0 0 0 / 2%)',
                cursor: 'pointer',
                ':hover': {
                  backgroundColor: '#ffffff',
                },
              }}>
                <CardBody></CardBody>
                <svg id="trophy" xmlns="http://www.w3.org/2000/svg" width="64.5" height="64.5" viewBox="0 0 64.5 64.5">
                  <path id="Path_40" data-name="Path 40" d="M393.888,56.455a1.89,1.89,0,0,1-1.05-3.462l8.813-5.876a3.773,3.773,0,0,0,1.683-3.144V37.559a3.779,3.779,0,0,0-7.559,0,1.89,1.89,0,1,1-3.779,0,7.559,7.559,0,0,1,15.117,0v6.414a7.542,7.542,0,0,1-3.366,6.289l-8.813,5.876A1.888,1.888,0,0,1,393.888,56.455Z" transform="translate(-342.613 -26.221)" fill="#ff9f00" />
                  <path id="Path_41" data-name="Path 41" d="M13.226,56.455a1.888,1.888,0,0,1-1.046-.317L3.366,50.262A7.542,7.542,0,0,1,0,43.973V37.559a7.559,7.559,0,0,1,15.117,0,1.89,1.89,0,0,1-3.779,0,3.779,3.779,0,1,0-7.559,0v6.414a3.773,3.773,0,0,0,1.683,3.145l8.813,5.876a1.89,1.89,0,0,1-1.05,3.462Z" transform="translate(0 -26.221)" fill="#fdbf00" />
                  <path id="Path_42" data-name="Path 42" d="M130.683,0H92.89A1.871,1.871,0,0,0,91,1.89V24.565c0,6.387,3.4,11.489,9.6,14.4a9.459,9.459,0,0,1,5.518,8.529,1.871,1.871,0,0,0,1.89,1.89h7.559a1.871,1.871,0,0,0,1.89-1.89v-.6a9.117,9.117,0,0,1,4.384-7.811c6.463-3.666,10.733-7.521,10.733-14.512V1.89A1.871,1.871,0,0,0,130.683,0Z" transform="translate(-79.536)" fill="#ffe470" />
                  <path id="Path_43" data-name="Path 43" d="M276.786,1.89V24.565c0,6.992-4.271,10.847-10.733,14.512a9.117,9.117,0,0,0-4.384,7.811v.6a1.871,1.871,0,0,1-1.89,1.89H256V0h18.9A1.871,1.871,0,0,1,276.786,1.89Z" transform="translate(-223.75)" fill="#ffd400" />
                  <path id="Path_44" data-name="Path 44" d="M171.786,362H156.669A5.661,5.661,0,0,0,151,367.669v3.779a1.871,1.871,0,0,0,1.89,1.89h22.676a1.871,1.871,0,0,0,1.89-1.89v-3.779A5.661,5.661,0,0,0,171.786,362Z" transform="translate(-131.978 -316.396)" fill="#384949" />
                  <path id="Path_45" data-name="Path 45" d="M149.345,422H126.669A5.661,5.661,0,0,0,121,427.669v3.779a1.871,1.871,0,0,0,1.89,1.89h30.234a1.871,1.871,0,0,0,1.89-1.89v-3.779A5.661,5.661,0,0,0,149.345,422Z" transform="translate(-105.757 -368.838)" fill="#4a696f" />
                  <path id="Path_46" data-name="Path 46" d="M188.694,68.687a1.822,1.822,0,0,0-1.549-1.285l-5.518-.832-2.456-4.989a1.963,1.963,0,0,0-3.4,0l-2.456,4.989-5.518.832a1.872,1.872,0,0,0-1.058,3.212l4.006,3.893-.945,5.48a1.889,1.889,0,0,0,2.721,2l4.951-2.608,4.951,2.608a1.78,1.78,0,0,0,1.965-.151,1.859,1.859,0,0,0,.756-1.852l-.945-5.48,4.006-3.893A1.839,1.839,0,0,0,188.694,68.687Z" transform="translate(-145.22 -52.965)" fill="#fdbf00" />
                  <path id="Path_47" data-name="Path 47" d="M260.951,81.99,256,79.382V60.6a1.906,1.906,0,0,1,1.7.983l2.456,4.989,5.518.832a1.872,1.872,0,0,1,1.058,3.212l-4.006,3.893.945,5.48a1.859,1.859,0,0,1-.756,1.852A1.78,1.78,0,0,1,260.951,81.99Z" transform="translate(-223.75 -52.965)" fill="#ff9f00" />
                  <path id="Path_48" data-name="Path 48" d="M269.228,367.669v3.779a1.871,1.871,0,0,1-1.89,1.89H256V362h7.559A5.661,5.661,0,0,1,269.228,367.669Z" transform="translate(-223.75 -316.396)" fill="#293939" />
                  <path id="Path_49" data-name="Path 49" d="M273.007,427.669v3.779a1.871,1.871,0,0,1-1.89,1.89H256V422h11.338A5.661,5.661,0,0,1,273.007,427.669Z" transform="translate(-223.75 -368.838)" fill="#3e5959" />
                </svg>
  
  
                <Header as="h4" content="Applaud  Cards" style={{
                  paddingTop: '20px',
                  marginBottom: '0'
                }} />
                {/* <Button circular icon={<ArrowRightIcon />} text title="Arrow" />   */}
                <Flex hAlign="end" vAlign="end" gap="gap.smaller">
                  <Button className="curve-btn" icon={<ArrowRightIcon />} title="Microphone" onClick={()=>this.card()}/>
                </Flex>
              </Card>
            </Flex>
            <Flex gap="gap.small" padding="padding.medium">
              <Card size="small" styles={{
                borderRadius: '6px',
                padding: '10px 20px 40px',
                backgroundColor: '#ffffff',
                boxShadow: '0px 10px 13px rgb(0 0 0 / 2%)',
                cursor: 'pointer',
                ':hover': {
                  backgroundColor: '#ffffff',
                },
              }}>
                <CardBody></CardBody>
                <svg xmlns="http://www.w3.org/2000/svg" width="65.125" height="65.125" viewBox="0 0 65.125 65.125">
                  <defs>
                    <linearGradient id="linear-gradient" x1="0.5" y1="1" x2="0.5" gradientUnits="objectBoundingBox">
                      <stop offset="0" stop-color="#6264a7" />
                      <stop offset="1" stop-color="#ff5700" />
                    </linearGradient>
                  </defs>
                  <g id="_2698011" data-name="2698011" transform="translate(0 0)">
                    <path id="Path_164" data-name="Path 164" d="M36.378,65.125H28.747a1.908,1.908,0,0,1-1.883-1.6l-1.173-7.148a24.585,24.585,0,0,1-5.108-2.121l-5.8,4.142a1.908,1.908,0,0,1-2.458-.2l-5.4-5.4a1.908,1.908,0,0,1-.2-2.458l4.142-5.8a24.584,24.584,0,0,1-2.121-5.108L1.6,38.261A1.908,1.908,0,0,1,0,36.378V28.747a1.908,1.908,0,0,1,1.6-1.883l7.148-1.173a24.585,24.585,0,0,1,2.121-5.108l-4.142-5.8a1.908,1.908,0,0,1,.2-2.458l5.4-5.4a1.908,1.908,0,0,1,2.458-.2l5.8,4.142a24.585,24.585,0,0,1,5.108-2.121L26.864,1.6A1.908,1.908,0,0,1,28.747,0h7.632a1.908,1.908,0,0,1,1.883,1.6l1.173,7.148a24.585,24.585,0,0,1,5.108,2.121l5.8-4.142a1.908,1.908,0,0,1,2.458.2l5.4,5.4a1.908,1.908,0,0,1,.2,2.458l-4.142,5.8a24.59,24.59,0,0,1,2.121,5.108l7.148,1.173a1.908,1.908,0,0,1,1.6,1.883v7.632a1.908,1.908,0,0,1-1.6,1.883l-7.148,1.173a24.577,24.577,0,0,1-2.121,5.108l4.142,5.8a1.908,1.908,0,0,1-.2,2.458l-5.4,5.4a1.908,1.908,0,0,1-2.458.2l-5.8-4.142a24.588,24.588,0,0,1-5.108,2.121l-1.173,7.148a1.908,1.908,0,0,1-1.883,1.6ZM63.217,36.378h0Z" fill="url(#linear-gradient)" />
                    <path id="Path_165" data-name="Path 165" d="M164.356,177.711a13.356,13.356,0,1,1,13.356-13.356A13.371,13.371,0,0,1,164.356,177.711Z" transform="translate(-131.793 -131.793)" fill="#fff" />
                  </g>
                </svg>
  
                <Header as="h4" content="Settings" style={{
                  paddingTop: '20px',
                  marginBottom: '0'
                }} />
                <Flex hAlign="end" vAlign="end" gap="gap.smaller">
                  <Button className="curve-btn" icon={<ArrowRightIcon />} title="Microphone" onClick={()=>this.setting()} />
                </Flex>
              </Card>
            </Flex>
          </Flex>
  
        </div>
        );
    }
}



export default AdminPreview;


