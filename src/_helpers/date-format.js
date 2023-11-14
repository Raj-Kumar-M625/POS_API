export function formatDateAndTime(inputDate) {
  const date = new Date(inputDate);

  const formattedDate = date.toLocaleString("en-GB", {
    day: "numeric",
    month: "short",
    year: "numeric",
    hour: "numeric",
    minute: "2-digit",
  });

  return formattedDate;
}

export function formatDate(inputDate) {
  const date = new Date(inputDate);

  const formattedDate = date.toLocaleString("en-GB", {
    day: "numeric",
    month: "short",
    year: "numeric",
  });

  return formattedDate;
}

export function formatTime(inputDate) {
  const date = new Date(inputDate);
  const formattedTime = date.toLocaleString("en-GB", {
    hour: "numeric",
    minute: "2-digit",
  });

  return formattedTime;
}
