import { useEffect, useState } from "react";
import { getClosedTickets, restoreTicket } from "../../managers/ticketManager";

export default function ClosedTickets() {
  const [tickets, setTickets] = useState([]);

  useEffect(() => {
    getClosedTickets()
      .then((data) => setTickets(data))
      .catch((error) => console.error("Error fetching tickets:", error));
  }, []);

  const handleReopenTicket = (ticketId) => {
    restoreTicket(ticketId)
      .then(() => {
        setTickets((prevTickets) =>
          prevTickets.filter((ticket) => ticket.id !== ticketId)
        );
      })
      .catch((error) => console.error("Error restoring ticket:", error));
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
                  <span className="text-success fw-bold">{ticket.status}</span>
                </td>
                <td className="text-end col-1">
                  <button
                    className="btn btn-primary btn-sm"
                    onClick={() => handleReopenTicket(ticket.id)}
                  >
                    Restore
                  </button>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      ) : (
        <p>No closed tickets available.</p>
      )}
    </div>
  );
}
