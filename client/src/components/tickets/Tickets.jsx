import { Link, Route, Routes } from "react-router-dom";
import OpenTickets from "./OpenTickets";
import ClosedTickets from "./ClosedTickets";
import NewTicket from "./NewTicket";
import SingleTicket from "./SingleTicket";
import "../../assets/styles/tickets.css";
import { useEffect, useState } from "react";
import { getOpenTickets } from "../../managers/ticketManager";

export default function Tickets({ loggedInUser }) {
  // State to store the count of open tickets
  const [openTicketCount, setOpenTicketCount] = useState(0);

  // Fetch the number of open tickets
  const fetchOpenTicketCount = async () => {
    try {
      const tickets = await getOpenTickets();
      setOpenTicketCount(tickets.length);
    } catch (error) {
      console.error("Error fetching open ticket count:", error);
    }
  };

  useEffect(() => {
    fetchOpenTicketCount();
  }, []);

  return (
    <div className="d-flex justify-content-between ticket mt-5 ticket-container">
      {/* Sidebar for Navigation */}
      <div className="col-3 border p-3 ticket-nav">
        <div>
          {/* New Ticket Button */}
          <Link
            to="/tickets/new-ticket"
            className="d-flex justify-content-end text-decoration-none"
          >
            <button className="btn d-block w-25 text-start mb-3 btn-success">
              <i className="bi bi-plus-circle me-3"></i>Create
            </button>
          </Link>

          {/* Open Tickets Button */}
          <Link to="/tickets/open-tickets" className="text-decoration-none">
            <button className="btn d-block w-100 text-start mb-2 text-white d-flex justify-content-between">
              <div>
                <i className="bi bi-inbox me-3"></i>Open Tickets
              </div>
              {openTicketCount > 0 && (
                <span className="badge bg-primary ms-2">{openTicketCount}</span>
              )}
            </button>
          </Link>
          <hr />

          {/* Closed Tickets Button */}
          <Link to="/tickets/closed-tickets" className="text-decoration-none">
            <button className="btn d-block w-100 text-start text-white">
              <i className="bi bi-trash3 me-3"></i>Trash
            </button>
          </Link>
        </div>
      </div>

      {/* Main Content */}
      <div className="col-9 border ticket-body1">
        <Routes>
          <Route
            path="new-ticket"
            element={<NewTicket loggedInUser={loggedInUser} />}
          />
          {/* Passing fetchOpenTicketCount to OpenTickets */}
          <Route
            path="open-tickets"
            element={<OpenTickets onTicketChange={fetchOpenTicketCount} />}
          />
          <Route
            path="closed-tickets"
            element={<ClosedTickets onTicketChange={fetchOpenTicketCount} />}
          />
          <Route path="ticket/:ticketId" element={<SingleTicket />} />{" "}
          {/* Add route for SingleTicket */}
        </Routes>
      </div>
    </div>
  );
}
