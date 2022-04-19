import React, {Component} from "react";
import {appsettings} from "../../App/appsettings"
import Cookies from 'js-cookie'
import Select from 'react-select'

export class Account extends Component{
    constructor(props){
        super(props);
        this.state = {
            accountModel:{},
            user:{},
            roles:this.props.roles,
            Balance: 0,
            addBalanceModel:{},
            Email:'',
            FirstName:'',
            Surname:'',
            TimeZone:'',
            options:[]
        }
    }

    initAccountModel(){
        fetch(appsettings.PurchaseFlowApiAddress + 'account/personalaccount', {
            method:"GET",
            headers:{
                'Accept':'application/json',
                'Content-Type':'application/json',
                'authorization': Cookies.get('JwtTokenCookie')
            }
        }).then(response => response.json()).then(data => this.setState({accountModel: data}));
    }

    initUser(){
        fetch(appsettings.UserApiAddress + 'account/getuser', {
            method:"GET",
            headers:{
                'Accept':'application/json',
                'Content-Type':'application/json',
                'authorization': Cookies.get('JwtTokenCookie')
            }
        }).then(response => response.json()).then(data => this.setState({user: data}));
    }

    changeBalance = (e) =>{
        if (e.target.value >= 0){
            this.setState({
                Balance: e.target.value
            });
        }
    }

    changeEmail = (e) =>{
        this.setState({
            Email: e.target.value
        });
    }

    changeFirstName = (e) =>{
        this.setState({
            FirstName: e.target.value
        });
    }

    changeSurname = (e) =>{
        this.setState({
            Surname: e.target.value
        });
    }

    changeTimeZone = (e) =>{
        this.setState({
            TimeZone:e.value
        });
    }

    editClick(){
        fetch(appsettings.UserApiAddress + 'account/edit/' + this.state.user.id, {
            method:"GET",
            headers:{
                'Accept':'application/json',
                'Content-Type':'application/json',
                'authorization': Cookies.get('JwtTokenCookie')
            }
        }).then(response => response.json()).then(data => {
            this.setState({
                Email:data.email,
                FirstName:data.firstName,
                Surname:data.surname,
                TimeZone:data.timeZone
            });
            var options = [];
            data.timeZones.forEach(timeZone => options.push({ value: timeZone.text, label: timeZone.text}))
            this.setState({options: options})
        })
    };

    onEditClick(){
        fetch(appsettings.UserApiAddress + 'account/edit',{
            method:'PUT',
            headers:{
                'Accept':'application/json',
                'Content-Type':'application/json',
                'authorization': Cookies.get('JwtTokenCookie')
            },
            body:JSON.stringify({
                Id: this.state.user.id,
                Email:this.state.Email,
                FirstName:this.state.FirstName,
                Surname:this.state.Surname,
                TimeZone:this.state.TimeZone
            })
        }).then(()=>{
            this.refresh();
        })
    }

    onAddBalanceClick(){
        fetch(appsettings.UserApiAddress + 'account/addbalance/' + this.state.user.id, {
            method:"GET",
            headers:{
                'Accept':'application/json',
                'Content-Type':'application/json',
                'authorization': Cookies.get('JwtTokenCookie')
            }
        }).then(response => response.json()).then(data => {
            fetch(appsettings.UserApiAddress + 'account/addbalance', {
                method:'PUT',
                headers:{
                    'Accept':'application/json',
                    'Content-Type':'application/json',
                    'authorization': Cookies.get('JwtTokenCookie')
                },
                body:JSON.stringify({
                    Id: data.id,
                    Balance: this.state.Balance
                })
            });
        }).then(() => this.refresh())
    }

    refresh(){
        this.initAccountModel();
        this.initUser();
    }

    componentDidMount(){
        this.refresh();
    }

    render(){
        const {
            accountModel,
            user,
            roles,
            Balance,
            Email,
            FirstName,
            Surname,
            options,
            TimeZone
        }=this.state;
        
        return(
            <div>
                <h1>Personal account</h1>
                <div>
                    <hr />
                    <dl className="row">
                        <dt className = "col-sm-2">
                            Email
                        </dt>
                        <dd className = "col-sm-10">
                            {user.email}
                        </dd>
                        <dt className = "col-sm-2">
                            Name
                        </dt>
                        <dd className = "col-sm-10">
                            {user.firstName}
                        </dd>
                        <dt className = "col-sm-2">
                            Surname
                        </dt>
                        <dd className = "col-sm-10">
                            {user.surname}
                        </dd>
                        <dt className = "col-sm-2">
                            Balance
                        </dt>
                        <dd className = "col-sm-10">
                            {user.balance}
                        </dd>
                        <dt className = "col-sm-2">
                            Timezone
                        </dt>
                        <dd className = "col-sm-10">
                            {user.timeZone}
                        </dd>
                    </dl>
                </div>
                {roles.includes("admin") || roles.includes("event manager") || roles.includes("user") || roles.includes("venue manager")?
                <div>
                    <button type="button" className="btn btn-primary" data-bs-toggle="modal"
                            data-bs-target="#modalBalanceWindow">
                        Add balance
                    </button>
                    <button type="button" className="btn btn-primary" data-bs-toggle="modal"
                            data-bs-target="#modalEditWindow" onClick={()=>this.editClick()}>
                        Edit
                    </button>
                    <hr />
                </div>:null}
                {accountModel.tickets !== undefined && accountModel.tickets.length>0?
                    <div>
                        <table className="table">
                            <thead>
                                <tr>
                                    <th>Event</th>
                                    <th>TimeStart</th>
                                    <th>TimeEnd</th>
                                    <th>Price</th>
                                </tr>
                            </thead>
                            <tbody>
                            {accountModel.tickets.map(ticket =>
                                <tr key={ticket.event}>
                                    <td>{ticket.event.name}</td>
                                    <td>{ticket.event.timeStart}</td>
                                    <td>{ticket.event.timeEnd}</td>
                                    <td>{ticket.price}$</td>
                                </tr>
                            )}
                            </tbody>
                        </table>
                    </div>:null}

                <div className="modal fade" id="modalBalanceWindow" tabIndex="-1" aria-hidden="true">
                    <div className="modal-dialog modal-lg modal-dialog-centered">
                        <div className="modal-content">
                            <div className="modal-header">
                                <h5 className="modal-title">Add balance</h5>
                                <button type="button" className="btn-close" data-bs-dismiss="modal" aria-label="Close"/>
                            </div>

                            <div className="modal-body">
                                <div className="input-group mb-3">
                                    <span className="input-group-text">Balance</span>
                                    <input type="number" className="form-control" value={Balance} onChange={this.changeBalance}/>
                                </div>
                                <button type="button" className="btn btn-primary float-start" onClick={()=>this.onAddBalanceClick()}>
                                    Add</button>
                            </div>
                        </div>
                    </div>
                </div>

                <div className="modal fade" id="modalEditWindow" tabIndex="-1" aria-hidden="true">
                    <div className="modal-dialog modal-lg modal-dialog-centered">
                        <div className="modal-content">
                            <div className="modal-header">
                                <h5 className="modal-title">Edit account</h5>
                                <button type="button" className="btn-close" data-bs-dismiss="modal" aria-label="Close"/>
                            </div>

                            <div className="modal-body">
                                <div className="input-group mb-3">
                                    <div className="input-group-text col">Email</div>
                                    <input type="text" className="form-control col" value={Email} onChange={this.changeEmail}/>
                                </div>
                                <div className="input-group mb-3">
                                    <span className="input-group-text col">FirstName</span>
                                    <input type="text" className="form-control col" value={FirstName} onChange={this.changeFirstName}/>
                                </div>
                                <div className="input-group mb-3">
                                    <span className="input-group-text col">Surname</span>
                                    <input type="text" className="form-control col" value={Surname} onChange={this.changeSurname}/>
                                </div>
                                <div className="input-group mb-3">
                                    <span className="input-group-text col">TimeZone</span>
                                    <Select className="form-control" defaultValue={{ value: TimeZone, label: TimeZone }} options={options} onChange={this.changeTimeZone} />
                                </div>
                                <button type="button" className="btn btn-primary float-start" onClick={()=>this.onEditClick()}>
                                    Edit</button>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        )
    }
}