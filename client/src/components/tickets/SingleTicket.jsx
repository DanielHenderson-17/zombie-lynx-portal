import { useParams, useNavigate } from "react-router-dom";
import { useEffect, useState } from "react";
import {
  getTicketById,
  closeTicketAPI,
  restoreTicketAPI,
  deleteTicket,
} from "../../managers/ticketManager";
import { formatLongDateTime } from "../../utils/longDateTime";
import { categoryFormatter } from "../../utils/categoryFormater";
import { getGameImage } from "../../utils/gameFormatter";

export default function SingleTicket() {
  const { ticketId } = useParams();
  const [ticket, setTicket] = useState(null);
  const [error, setError] = useState(null);
  const navigate = useNavigate();

  // Fetch the ticket details
  useEffect(() => {
    const fetchTicket = async () => {
      try {
        const data = await getTicketById(ticketId);
        setTicket(data);
      } catch (error) {
        console.error("Error fetching ticket:", error);
        setError("Failed to fetch ticket details.");
      }
    };

    fetchTicket();
  }, [ticketId]);

  // Close the ticket
  const handleCloseTicket = async () => {
    try {
      await closeTicketAPI(ticketId);
      navigate("/tickets/closed-tickets");
    } catch (error) {
      console.error("Error closing ticket:", error);
      setError("Failed to close the ticket. Please try again.");
    }
  };

  // Restore the ticket
  const handleRestoreTicket = async () => {
    try {
      await restoreTicketAPI(ticketId);
      navigate("/tickets");
    } catch (error) {
      console.error("Error restoring ticket:", error);
      setError("Failed to restore the ticket. Please try again.");
    }
  };

  // Delete the ticket
  const handleDeleteTicket = async () => {
    try {
      await deleteTicket(ticketId);
      navigate("/tickets/closed-tickets");
    } catch (error) {
      console.error("Error deleting ticket:", error);
      setError("Failed to delete the ticket. Please try again.");
    }
  };

  if (!ticket) {
    return <p>Loading ticket details...</p>;
  }

  return (
    <div className="text-white col-6 mx-auto mt-5 pt-3">
      <h2 className="text-start mb-1 fs-2">{ticket.subject}</h2>
      <div className="d-flex justify-content-between mb-1">
        <div className="d-flex align-items-center fs-5">
          <div
            className="me-2"
            dangerouslySetInnerHTML={{
              __html: categoryFormatter(ticket.category),
            }}
          ></div>
          {ticket.category}
        </div>
        <div className="d-flex align-items-center">
          <img
            className="gameImg me-2"
            src={getGameImage(ticket.game)}
            alt=""
          />{" "}
          {ticket.server}
        </div>
      </div>
      <small className="text-start d-block mb-3" style={{ fontSize: "0.7rem" }}>
        <i className="bi bi-calendar-date me-2"></i>
        {formatLongDateTime(ticket.updatedAt)}
      </small>
      <div className="text-start">
        <strong className="text-start">Description:</strong>{" "}
        <p className="border rounded-2 p-3 mt-2">{ticket.description}</p>
      </div>
      <p className="d-flex justify-content-between">
        <div className="d-flex align-items-center">
          <div className="text-start me-3">Assigned:</div>{" "}
          {ticket.assignedUsers.map((user) => (
            <span key={user.id}>
              {user.firstName} {user.lastName}
              {ticket.assignedUsers.indexOf(user) !==
              ticket.assignedUsers.length - 1
                ? ", "
                : ""}
            </span>
          ))}
        </div>
        <div className="d-flex justify-content-end align-items-center">
          {ticket.status === "Open" ? (
            <button className="btn btn-danger" onClick={handleCloseTicket}>
              Close Ticket <i className="bi bi-x-circle ms-2"></i>
            </button>
          ) : (
            <>
              <button
                className="btn btn-primary me-2"
                onClick={handleRestoreTicket}
              >
                Restore Ticket{" "}
                <i className="bi bi-arrow-counterclockwise ms-2"></i>
              </button>
              <button className="btn btn-danger" onClick={handleDeleteTicket}>
                Delete Ticket <i className="bi bi-trash3 ms-2"></i>
              </button>
            </>
          )}
        </div>
      </p>

      {error && <p className="text-danger mt-3">{error}</p>}
    </div>
  );
}
