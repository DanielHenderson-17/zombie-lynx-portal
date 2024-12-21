import { useEffect, useState } from "react";
import { getOpenTickets, closeTicket } from "../../managers/ticketManager";

export default function OpenTickets() {
  const [tickets, setTickets] = useState([]);

  useEffect(() => {
    getOpenTickets()
      .then((data) => setTickets(data))
      .catch((error) => console.error("Error fetching tickets:", error));
  }, []);

  const handleCloseTicket = (ticketId) => {
    closeTicket(ticketId)
      .then(() => {
        setTickets((prevTickets) =>
          prevTickets.filter((ticket) => ticket.id !== ticketId)
        );
      })
      .catch((error) => console.error("Error closing ticket:", error));
  };

  return (
    <div>
      {tickets.length > 0 ? (
        <table className="table table-striped align-middle">
          <thead className="thead-dark">
            <tr>
              <th className="text-start col-4">Topic</th>
              <th className="text-end col-2">Game</th>
              <th className="text-end col-2">Server</th>
              <th className="text-end col-1">Status</th>
              <th className="text-end col-1">Options</th>
            </tr>
          </thead>
          <tbody>
            {tickets.map((ticket) => (
              <tr key={ticket.id}>
                <td className="text-start col-4">
                  <div>
                    <strong>{ticket.subject}</strong>
                    <br />
                    <small className="text-muted">{ticket.categroy}</small>
                    <br />
                    <small className="text-muted">
                      {new Date(ticket.createdAt).toLocaleString()}
                    </small>
                  </div>
                </td>
                <td className="text-end col-2">
                  <span className="text-warning fw-bold">{ticket.game}</span>
                </td>
                <td className="text-end col-2">
                  <span className="text-warning fw-bold">{ticket.server}</span>
                </td>
                <td className="text-end col-1">
                  <span className="text-warning fw-bold">{ticket.status}</span>
                </td>
                <td className="text-end col-1">
                  <button
                    className="btn btn-danger btn-sm"
                    onClick={() => handleCloseTicket(ticket.id)}
                  >
                    Close
                  </button>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      ) : (
        <p>No open tickets available.</p>
      )}
    </div>
  );
}
