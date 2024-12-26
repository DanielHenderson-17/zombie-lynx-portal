import { useEffect, useState } from "react";
import {
  getClosedTickets,
  restoreTicketAPI,
  deleteTicket,
} from "../../managers/ticketManager";

export default function ClosedTickets() {
  const [tickets, setTickets] = useState([]);
  const [error, setError] = useState(null);

  useEffect(() => {
    getClosedTickets()
      .then((data) => {
        setTickets(data);
      })
      .catch((error) => {
        console.error("Error fetching closed tickets:", error);
        setError("Failed to fetch closed tickets. Please try again.");
      });
  }, []);

  const handleRestoreTicket = (ticketId) => {
    restoreTicketAPI(ticketId)
      .then(() => {
        getClosedTickets()
          .then((updatedTickets) => setTickets(updatedTickets))
          .catch((error) =>
            console.error("Error fetching updated tickets:", error)
          );
      })
      .catch((error) => console.error("Error restoring ticket:", error));
  };

  const handleDeleteTicket = (ticketId) => {
    deleteTicket(ticketId)
      .then(() => {
        setTickets((prevTickets) =>
          prevTickets.filter((ticket) => ticket.id !== ticketId)
        );
      })
      .catch((error) => console.error("Error deleting ticket:", error));
  };

  if (error) {
    return <p className="text-danger">{error}</p>;
  }

  return (
    <div className="col-12 ticket-body">
      <table className="table table-striped align-middle">
        <thead className="thead-dark">
          <tr>
            <th className="text-start col-4">Topic</th>
            <th className="text-start col-1">Game</th>
            <th className="text-start col-2">Server</th>
            <th className="text-start col-1">Status</th>
            <th className="text-end col-2 pe-3">Options</th>
          </tr>
        </thead>
        <tbody>
          {tickets.map((ticket) => (
            <tr key={ticket.id}>
              <td className="text-start col-4">
                <div>
                  <strong>{ticket.subject}</strong>
                  <br />
                  <small className="text-muted">{ticket.category}</small>
                  <br />
                  <small className="text-muted">
                    {new Date(ticket.createdAt).toLocaleString()}
                  </small>
                  <br />
                  <small className="text-muted">
                    Assigned:{" "}
                    {ticket.assignedUsers
                      .map((user) => `${user.firstName} ${user.lastName}`)
                      .join(", ")}{" "}
                  </small>
                </div>
              </td>
              <td className="text-start col-1">
                <span className="text-warning fw-bold">{ticket.game}</span>
              </td>
              <td className="text-start col-2">
                <span className="text-warning fw-bold">{ticket.server}</span>
              </td>
              <td className="text-start col-1">
                <span className="text-success fw-bold">{ticket.status}</span>
              </td>
              <td className="text-start col-2 position-relative">
                <div className="d-flex justify-content-end pe-2">
                  <button
                    className="btn btn-primary btn-sm ticket-button me-2"
                    onClick={() => handleRestoreTicket(ticket.id)}
                  >
                    Restore
                  </button>
                  <button
                    className="btn btn-danger btn-sm ticket-button"
                    onClick={() => handleDeleteTicket(ticket.id)}
                  >
                    Delete
                  </button>
                </div>
                <small className="position-absolute ticket-id">
                  Ticket ID: {ticket.id}
                </small>
              </td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
}
