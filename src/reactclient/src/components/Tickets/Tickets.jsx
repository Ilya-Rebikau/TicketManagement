import React, {Component} from "react";
import {appsettings} from "../../App/appsettings"
import Cookies from 'js-cookie'
import { Page } from "../Page";

export class Tickets extends Component{
    constructor(props){
        super(props);
        this.state = {
            tickets:[],
            modalTitle:"",
            Id:0,
            EventSeatId:0,
            UserId:"",
            nextTickets:[]
        }
        Page.number = 1;
    }

    getNextPage(){
        var nextPageNumber = Page.number;
        nextPageNumber++;
        var urlNext = new URL(appsettings.EventManagerApiAddress + 'tickets/gettickets'),
        paramsNext = {pageNumber:nextPageNumber}
        Object.keys(paramsNext).forEach(key => urlNext.searchParams.append(key, paramsNext[key]))
        fetch(urlNext, {
            method:"GET",
            headers:{
                'Accept':'application/json',
                'Content-Type':'application/json',
                'authorization': Cookies.get('JwtTokenCookie')
            }
        }).then(response => response.json()).then(data => this.setState({ nextTickets: data }));
    }

    nextPage(){
        Page.number++;
        this.refreshList();
    }

    firstPage(){
        Page.number = 1;
        this.refreshList();
    }

    previousPage(){
        Page.number--;
        this.refreshList();
    }

    changeEventSeatId = (e) =>{
        this.setState({
            EventSeatId:e.target.value
        });
    }

    changeUserId = (e) =>{
        this.setState({
            UserId:e.target.value
        });
    }

    refreshList(){
        var url = new URL(appsettings.EventManagerApiAddress + 'tickets/gettickets'),
            params = {pageNumber:Page.number}
        Object.keys(params).forEach(key => url.searchParams.append(key, params[key]))
        fetch(url, {
            method:"GET",
            headers:{
                'Accept':'application/json',
                'Content-Type':'application/json',
                'authorization': Cookies.get('JwtTokenCookie')
            }
        }).then(response => response.json()).then(data => this.setState({ tickets: data}));
        this.getNextPage();
    }

    componentDidMount(){
        this.refreshList();
    }

    convertState(state){
        if (state === 0){
            return 'Free'
        }
        else{
            return 'Occupied'
        }
    }

    onCreateClick(){
        fetch(appsettings.EventManagerApiAddress + 'tickets/create',{
            method:'POST',
            headers:{
                'Accept':'application/json',
                'Content-Type':'application/json',
                'authorization': Cookies.get('JwtTokenCookie')
            },
            body:JSON.stringify({
                Id: 0,
                EventSeatId: this.state.EventSeatId,
                UserId: this.state.UserId,
                Price: 0,
            })
        }).then(response => {
            if (response.status === 400){
                alert('Http 400 error')
            }
        }).then(()=>{
            this.refreshList();
        })
    }

    onEditClick(id){
        fetch(appsettings.EventManagerApiAddress + 'tickets/edit/' + id,{
            method:'PUT',
            headers:{
                'Accept':'application/json',
                'Content-Type':'application/json',
                'authorization': Cookies.get('JwtTokenCookie')
            },
            body:JSON.stringify({
                Id: id,
                EventSeatId: this.state.EventSeatId,
                UserId: this.state.UserId
            })
        }).then(response => {
            if (response.status === 400){
                alert('Http 400 error')
            }
        }).then(()=>{
            this.refreshList();
        })
    }

    createClick(){
        this.setState({
            modalTitle:"Add event seat",
            EventSeatId:0,
            UserId:"",
            Id: 0
        });
    }

    editClick(ticket){
        this.setState({
            modalTitle:"Edit event seat",
            Id:ticket.id,
            EventSeatId:ticket.eventSeatId,
            UserId:ticket.userId
        });
    }

    deleteClick(id){
        if(window.confirm('Are you sure?')){
            fetch(appsettings.EventManagerApiAddress + 'tickets/delete/' + id,{
                method:'DELETE',
                headers:{
                    'Accept':'application/json',
                    'Content-Type':'application/json',
                    'authorization': Cookies.get('JwtTokenCookie')
            }}).then(response => {
                if (response.status === 400){
                    alert('Http 400 error')
                }
            })
            .then(()=>{
                this.refreshList();
            })
        }
    }

    render(){
        const {
            tickets,
            modalTitle,
            Id,
            EventSeatId,
            UserId,
            nextTickets
        }=this.state;

        return(
            <div>
                <p>
                    <button type="button"
                        className="btn btn-primary"
                        data-bs-toggle="modal"
                        data-bs-target="#modalWindow"
                        onClick={()=>this.createClick()}>
                            Add ticket
                    </button>
                </p>
                <table className="table">
                    <thead>
                        <tr>
                            <th>
                                EventSeatId
                            </th>
                            <th>
                                UserId
                            </th>
                            <th>
                                Operations
                            </th>
                        </tr>
                    </thead>
                    <tbody>
                        {tickets.map(ticket => 
                            <tr key={ticket.id}>
                                <td>
                                    {ticket.eventSeatId}
                                </td>
                                <td>
                                    {ticket.userId}
                                </td>
                                <td>
                                    <button type="button" className="btn btn-primary" data-bs-toggle="modal"
                                            data-bs-target="#modalWindow" onClick={()=>this.editClick(ticket)}>
                                        Edit
                                    </button>
                                    <button type="button" className="btn btn-danger" onClick={()=>this.deleteClick(ticket.id)}>
                                        Delete
                                    </button>
                                </td>
                            </tr>)}
                    </tbody>
                </table>

                <div>
                    {Page.number!==1?
                        <button type="button" className="btn btn-primary" onClick={() => this.firstPage()}>1</button>:null}
                    {Page.number > 1?
                        <button type="button" className="btn btn-primary" onClick={() => this.previousPage()}>&lt;</button>:null}
                    {nextTickets.length !== 0?
                        <button type="button" className="btn btn-primary" onClick={() => this.nextPage()}>&gt;</button>:null}
                </div>

                <div className="modal fade" id="modalWindow" tabIndex="-1" aria-hidden="true">
                    <div className="modal-dialog modal-lg modal-dialog-centered">
                        <div className="modal-content">
                            <div className="modal-header">
                                <h5 className="modal-title">{modalTitle}</h5>
                                <button type="button" className="btn-close" data-bs-dismiss="modal" aria-label="Close"/>
                            </div>

                            <div className="modal-body">
                                <div className="input-group mb-3">
                                    <span className="input-group-text">EventSeatId</span>
                                    <input type="number" className="form-control" value={EventSeatId} onChange={this.changeEventSeatId}/>
                                    <span className="input-group-text">UserId</span>
                                    <input type="text" className="form-control" value={UserId} onChange={this.changeUserId}/>
                                </div>
                                {Id===0?
                                <button type="button" className="btn btn-primary float-start"onClick={()=>this.onCreateClick()}>
                                    Create</button>:null}
                                {Id!==0?
                                <button type="button" className="btn btn-primary float-start" onClick={()=>this.onEditClick(Id)}>
                                    Update</button>:null}
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        )
    }
}